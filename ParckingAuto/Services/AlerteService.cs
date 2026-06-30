using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Enums;
using ParckingAuto.Models;
using ParckingAuto.Repositories;

namespace ParckingAuto.Services
{
    public class AlerteService
    {
        private static readonly SemaphoreSlim SyncLock = new(1, 1);

        private readonly AlerteRepository _repo;
        private readonly ParcAutoContext _context;
        private readonly ParametresService _parametresService;

        public AlerteService(AlerteRepository repo, ParcAutoContext context, ParametresService parametresService)
        {
            _repo = repo;
            _context = context;
            _parametresService = parametresService;
        }

        public async Task<List<Alerte>> GetAllAsync(bool includeResolues = false)
        {
            await SyncAlertsAsync();
            var list = await _repo.GetAllAsync();

            if (!includeResolues)
                list = list.Where(a => a.Statut != StatutAlerteEnum.Resolue).ToList();

            PopulateMessages(list);
            return list;
        }

        public async Task<Alerte?> ResoudreAsync(int id)
        {
            var alerte = await _context.Alertes
                .Include(a => a.Vehicule)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (alerte == null) return null;

            alerte.Statut = StatutAlerteEnum.Resolue;
            alerte.DateResolution = DateTime.Now;

            if (alerte.TypeAlerte == TypeAlerteEnum.Vidange && alerte.Vehicule != null)
                alerte.Vehicule.DernierKmVidange = alerte.Vehicule.Kilometrage;

            await _context.SaveChangesAsync();
            PopulateMessages(new List<Alerte> { alerte });
            return alerte;
        }

        public Task<Alerte?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<Alerte> AddAsync(Alerte a) => _repo.AddAsync(a);
        public Task UpdateAsync(Alerte a) => _repo.UpdateAsync(a);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        private static StatutAlerteEnum GetDateAlertStatut(int days)
        {
            if (days <= 7) return StatutAlerteEnum.Critique;
            return StatutAlerteEnum.PreAlerte;
        }

        private void PopulateMessages(List<Alerte> list)
        {
            var chauffeurs = _context.Chauffeurs.AsEnumerable().ToList();

            foreach (var alerte in list)
            {
                alerte.VehiculeImmatriculation = alerte.Vehicule != null ? alerte.Vehicule.Immatriculation : "N/A";

                if (alerte.Statut == StatutAlerteEnum.Resolue)
                {
                    alerte.Message = $"Traité le {alerte.DateResolution:dd/MM/yyyy} — action effectuée.";
                    continue;
                }

                if (alerte.TypeAlerte == TypeAlerteEnum.Vidange)
                {
                    var kmDepuis = alerte.Vehicule != null
                        ? alerte.Vehicule.Kilometrage - alerte.Vehicule.DernierKmVidange
                        : 0;
                    alerte.Message = alerte.Statut == StatutAlerteEnum.Critique
                        ? $"Alerte critique vidange ! {kmDepuis} km depuis la dernière vidange (seuil: 10 000 km)"
                        : $"Pré-alerte vidange. {kmDepuis} km depuis la dernière vidange (seuil: 9 000 km)";
                }
                else if (alerte.TypeAlerte == TypeAlerteEnum.Assurance)
                {
                    int days = alerte.Vehicule?.AssuranceDate.HasValue == true
                        ? (alerte.Vehicule.AssuranceDate!.Value.Date - DateTime.Today).Days
                        : 0;
                    var niveau = days <= 7 ? "J-7 critique" : days <= 15 ? "J-15" : "J-30";
                    alerte.Message = days >= 0
                        ? $"Assurance ({niveau}) : expiration dans {days} jours le {alerte.Vehicule?.AssuranceDate:dd/MM/yyyy}"
                        : $"Assurance expirée depuis {-days} jours le {alerte.Vehicule?.AssuranceDate:dd/MM/yyyy}";
                }
                else if (alerte.TypeAlerte == TypeAlerteEnum.VisiteTechnique)
                {
                    int days = alerte.Vehicule?.ProchaineVisite.HasValue == true
                        ? (alerte.Vehicule.ProchaineVisite!.Value.Date - DateTime.Today).Days
                        : 0;
                    var niveau = days <= 7 ? "J-7 critique" : days <= 15 ? "J-15" : "J-30";
                    alerte.Message = days >= 0
                        ? $"Visite technique ({niveau}) : échéance dans {days} jours le {alerte.Vehicule?.ProchaineVisite:dd/MM/yyyy}"
                        : $"Visite technique expirée depuis {-days} jours";
                }
                else if (alerte.TypeAlerte == TypeAlerteEnum.PermisChauffeur)
                {
                    var ch = chauffeurs.FirstOrDefault(c => c.PermisExpiration.Date == alerte.DateAlerte.Date);
                    int days = (alerte.DateAlerte.Date - DateTime.Today).Days;
                    var niveau = days <= 7 ? "J-7 critique" : days <= 15 ? "J-15" : "J-30";
                    alerte.Message = ch != null
                        ? $"Permis ({niveau}) : {ch.Prenom} {ch.Nom} expire dans {days} jours (N° {ch.PermisNumero})"
                        : $"Permis chauffeur ({niveau}) : expiration dans {days} jours";
                }
            }
        }

        private async Task SyncAlertsAsync()
        {
            await SyncLock.WaitAsync();
            try
            {
                var parametres = await _parametresService.GetAsync();
                var vehicles = await _context.Vehicules.AsNoTracking().ToListAsync();
                var chauffeurs = await _context.Chauffeurs.AsNoTracking().ToListAsync();

                var resolvedKeys = await _context.Alertes
                    .AsNoTracking()
                    .Where(a => a.Statut == StatutAlerteEnum.Resolue)
                    .Select(a => new { a.VehiculeId, a.TypeAlerte, a.ReferenceDeclencheur })
                    .ToListAsync();

                var resolvedSet = resolvedKeys
                    .Select(a => (a.VehiculeId, a.TypeAlerte, a.ReferenceDeclencheur))
                    .ToHashSet();

                await _context.Alertes
                    .Where(a => a.Statut != StatutAlerteEnum.Resolue)
                    .ExecuteDeleteAsync();

                var newAlerts = new List<Alerte>();

                foreach (var v in vehicles)
                {
                    if (parametres.NotifVidange)
                    {
                        var kmDepuis = v.Kilometrage - v.DernierKmVidange;
                        var refVidange = v.DernierKmVidange.ToString();
                        if (kmDepuis >= 10000 && !resolvedSet.Contains((v.Id, TypeAlerteEnum.Vidange, refVidange)))
                        {
                            newAlerts.Add(CreateAlert(v.Id, TypeAlerteEnum.Vidange, DateTime.Now, StatutAlerteEnum.Critique, refVidange));
                        }
                        else if (kmDepuis >= 9000 && !resolvedSet.Contains((v.Id, TypeAlerteEnum.Vidange, refVidange)))
                        {
                            newAlerts.Add(CreateAlert(v.Id, TypeAlerteEnum.Vidange, DateTime.Now, StatutAlerteEnum.PreAlerte, refVidange));
                        }
                    }

                    if (parametres.NotifAssurance && v.AssuranceDate.HasValue)
                    {
                        int days = (v.AssuranceDate.Value.Date - DateTime.Today).Days;
                        var refAssurance = v.AssuranceDate.Value.ToString("yyyy-MM-dd");
                        if (days <= 30 && !resolvedSet.Contains((v.Id, TypeAlerteEnum.Assurance, refAssurance)))
                        {
                            newAlerts.Add(CreateAlert(v.Id, TypeAlerteEnum.Assurance, v.AssuranceDate.Value, GetDateAlertStatut(days), refAssurance));
                        }
                    }

                    if (parametres.NotifVisiteTech && v.ProchaineVisite.HasValue)
                    {
                        int days = (v.ProchaineVisite.Value.Date - DateTime.Today).Days;
                        var refVisite = v.ProchaineVisite.Value.ToString("yyyy-MM-dd");
                        if (days <= 30 && !resolvedSet.Contains((v.Id, TypeAlerteEnum.VisiteTechnique, refVisite)))
                        {
                            newAlerts.Add(CreateAlert(v.Id, TypeAlerteEnum.VisiteTechnique, v.ProchaineVisite.Value, GetDateAlertStatut(days), refVisite));
                        }
                    }
                }

                if (parametres.NotifPermis)
                {
                    foreach (var c in chauffeurs)
                    {
                        int days = (c.PermisExpiration.Date - DateTime.Today).Days;
                        var refPermis = c.PermisExpiration.ToString("yyyy-MM-dd");
                        if (days <= 30)
                        {
                            var lastMvt = await _context.Mouvements
                                .AsNoTracking()
                                .Where(m => m.ChauffeurId == c.Id)
                                .OrderByDescending(m => m.DateDepart)
                                .FirstOrDefaultAsync();

                            int vehId = lastMvt?.VehiculeId ?? vehicles.FirstOrDefault()?.Id ?? 0;
                            if (vehId > 0 && !resolvedSet.Contains((vehId, TypeAlerteEnum.PermisChauffeur, refPermis)))
                            {
                                newAlerts.Add(CreateAlert(vehId, TypeAlerteEnum.PermisChauffeur, c.PermisExpiration, GetDateAlertStatut(days), refPermis));
                            }
                        }
                    }
                }

                if (newAlerts.Count > 0)
                {
                    await _context.Alertes.AddRangeAsync(newAlerts);
                    await _context.SaveChangesAsync();
                }
            }
            finally
            {
                SyncLock.Release();
            }
        }

        private static Alerte CreateAlert(int vehiculeId, TypeAlerteEnum type, DateTime date, StatutAlerteEnum statut, string reference)
        {
            return new Alerte
            {
                VehiculeId = vehiculeId,
                TypeAlerte = type,
                DateAlerte = date,
                Statut = statut,
                ReferenceDeclencheur = reference
            };
        }
    }
}
