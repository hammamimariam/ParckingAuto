using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.DTO;
using ParckingAuto.Enums;
using ParckingAuto.Models;
using ParckingAuto.Repositories;

namespace ParckingAuto.Services
{
    public class VehiculeService
    {
        private readonly VehiculeRepository _repo;
        private readonly ParcAutoContext _context;

        public VehiculeService(VehiculeRepository repo, ParcAutoContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<List<Vehicule>> GetAllAsync() => await _repo.GetAllAsync();
        public Task<Vehicule?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<Vehicule> AddAsync(Vehicule v)
        {
            if (v.DernierKmVidange <= 0)
                v.DernierKmVidange = v.Kilometrage;
            return await _repo.AddAsync(v);
        }

        public Task UpdateAsync(Vehicule v) => _repo.UpdateAsync(v);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public async Task<string> GetStatutAsync(int vehiculeId)
        {
            var enMission = await _context.Mouvements
                .AnyAsync(m => m.VehiculeId == vehiculeId && m.DateRetour == null);
            return enMission ? "En mission" : "Au parking";
        }

        public async Task<List<VehiculeDto>> ToDtoListAsync(IEnumerable<Vehicule> vehicules)
        {
            var idsEnMission = await _context.Mouvements
                .Where(m => m.DateRetour == null)
                .Select(m => m.VehiculeId)
                .ToListAsync();

            return vehicules.Select(v => MapToDto(v, idsEnMission.Contains(v.Id))).ToList();
        }

        public async Task<VehiculeSuiviDto?> GetSuiviAsync(int id)
        {
            var v = await _context.Vehicules.FindAsync(id);
            if (v == null) return null;

            var statut = await GetStatutAsync(id);
            var vehiculeDto = MapToDto(v, statut == "En mission");

            var mouvements = await _context.Mouvements
                .Include(m => m.Vehicule)
                .Include(m => m.Chauffeur)
                .Where(m => m.VehiculeId == id)
                .OrderByDescending(m => m.DateDepart)
                .Take(15)
                .ToListAsync();

            var carburants = await _context.Carburants
                .Include(c => c.Vehicule)
                .Where(c => c.VehiculeId == id)
                .OrderByDescending(c => c.DatePlein)
                .Take(15)
                .ToListAsync();

            var maintenances = await _context.Maintenances
                .Include(m => m.Vehicule)
                .Where(m => m.VehiculeId == id)
                .OrderByDescending(m => m.DateIntervention)
                .Take(15)
                .ToListAsync();

            var alertes = await _context.Alertes
                .Include(a => a.Vehicule)
                .Where(a => a.VehiculeId == id && a.Statut != StatutAlerteEnum.Resolue)
                .OrderByDescending(a => a.DateAlerte)
                .ToListAsync();

            var now = DateTime.Today;
            var coutMaintenanceMois = maintenances
                .Where(m => m.DateIntervention.Year == now.Year && m.DateIntervention.Month == now.Month)
                .Sum(m => m.Cout);
            var coutCarburantMois = carburants
                .Where(c => c.DatePlein.Year == now.Year && c.DatePlein.Month == now.Month)
                .Sum(c => c.Montant);
            var litresMois = carburants
                .Where(c => c.DatePlein.Year == now.Year && c.DatePlein.Month == now.Month)
                .Sum(c => (double)c.VolumeLitres);

            var orderedFuel = carburants.OrderBy(c => c.Kilometrage).ToList();
            double consoMoyenne = 0;
            if (orderedFuel.Count >= 2)
            {
                var litres = orderedFuel.Sum(c => (double)c.VolumeLitres);
                var km = orderedFuel.Last().Kilometrage - orderedFuel.First().Kilometrage;
                if (km > 0) consoMoyenne = Math.Round((litres * 100) / km, 2);
            }

            return new VehiculeSuiviDto
            {
                Vehicule = vehiculeDto,
                KmDepuisVidange = Math.Max(0, v.Kilometrage - v.DernierKmVidange),
                CoutMaintenanceMoisCourant = coutMaintenanceMois,
                CoutCarburantMoisCourant = coutCarburantMois,
                LitresMoisCourant = litresMois,
                ConsommationMoyenne = consoMoyenne,
                CoutMaintenanceTotal = maintenances.Sum(m => m.Cout),
                CoutCarburantTotal = carburants.Sum(c => c.Montant),
                DerniersMouvements = mouvements.Select(MapMouvement).ToList(),
                DerniersPleins = carburants.Select(MapCarburant).ToList(),
                DernieresMaintenances = maintenances.Select(MapMaintenance).ToList(),
                AlertesActives = alertes.Select(MapAlerte).ToList()
            };
        }

        private static VehiculeDto MapToDto(Vehicule v, bool enMission) => new()
        {
            Id = v.Id,
            Immatriculation = v.Immatriculation,
            Marque = v.Marque,
            Constructeur = string.IsNullOrWhiteSpace(v.Constructeur) ? v.Marque : v.Constructeur,
            TypeConstructeur = v.TypeConstructeur,
            Modele = v.Modele,
            TypeCommercial = v.TypeCommercial,
            AnneeFabrication = v.AnneeFabrication,
            AnneeMiseEnCirculation = v.AnneeMiseEnCirculation,
            TypeCarburant = v.TypeCarburant.ToString(),
            NumeroChassis = v.NumeroChassis,
            NumeroSerieType = v.NumeroSerieType,
            Kilometrage = v.Kilometrage,
            DernierKmVidange = v.DernierKmVidange,
            KmDepuisVidange = Math.Max(0, v.Kilometrage - v.DernierKmVidange),
            NumeroCarteGrise = v.NumeroCarteGrise,
            GenreVehicule = v.GenreVehicule,
            Carrosserie = v.Carrosserie,
            PuissanceFiscale = v.PuissanceFiscale,
            Cylindree = v.Cylindree,
            PTAC = v.PTAC,
            NombreEssieux = v.NombreEssieux,
            ChargeUtile = v.ChargeUtile,
            NombrePlacesDebout = v.NombrePlacesDebout,
            ImmatriculationPrecedente = v.ImmatriculationPrecedente,
            Restrictions = v.Restrictions,
            DateEtablissementCarteGrise = v.DateEtablissementCarteGrise,
            LieuEtablissementCarteGrise = v.LieuEtablissementCarteGrise,
            NombrePlaces = v.NombrePlaces,
            Couleur = v.Couleur,
            DatePremiereMiseEnCirculation = v.DatePremiereMiseEnCirculation,
            Assurance = v.CompagnieAssurance,
            AssuranceReference = v.AssuranceReference,
            AssuranceDateDebut = v.AssuranceDateDebut,
            AssuranceDate = v.AssuranceDate,
            UsageVehicule = v.UsageVehicule,
            VisiteTechniqueDate = v.VisiteTechniqueDate,
            ProchaineVisite = v.ProchaineVisite,
            Statut = enMission ? "En mission" : "Au parking"
        };

        private static MouvementDto MapMouvement(Mouvement m) => new()
        {
            Id = m.Id,
            VehiculeId = m.VehiculeId,
            ChauffeurId = m.ChauffeurId,
            DateDepart = m.DateDepart,
            DateRetour = m.DateRetour,
            KmDepart = m.KmDepart,
            KmRetour = m.KmRetour,
            Destination = m.Destination,
            VehiculeImmatriculation = m.Vehicule?.Immatriculation ?? "",
            ChauffeurNomComplet = m.Chauffeur != null ? $"{m.Chauffeur.Prenom} {m.Chauffeur.Nom}" : ""
        };

        private static CarburantDto MapCarburant(Carburant c) => new()
        {
            Id = c.Id,
            VehiculeId = c.VehiculeId,
            Date = c.DatePlein,
            Litres = (double)c.VolumeLitres,
            Cout = (double)c.Montant,
            Kilometrage = c.Kilometrage,
            VehiculeImmatriculation = c.Vehicule?.Immatriculation ?? ""
        };

        private static MaintenanceDto MapMaintenance(Models.Maintenance m) => new()
        {
            Id = m.Id,
            VehiculeId = m.VehiculeId,
            TypeIntervention = m.TypeIntervention,
            Description = m.Description,
            DateIntervention = m.DateIntervention,
            KilometrageIntervention = m.KilometrageIntervention,
            Cout = m.Cout,
            Fournisseur = m.Fournisseur,
            Facture = m.Facture,
            VehiculeImmatriculation = m.Vehicule?.Immatriculation ?? "",
            VehiculeMarque = m.Vehicule?.Marque ?? "",
            VehiculeModele = m.Vehicule?.Modele ?? ""
        };

        private static AlerteDto MapAlerte(Alerte a) => new()
        {
            Id = a.Id,
            VehiculeId = a.VehiculeId,
            TypeAlerte = a.TypeAlerte.ToString(),
            DateAlerte = a.DateAlerte,
            Statut = a.Statut.ToString(),
            VehiculeImmatriculation = a.Vehicule?.Immatriculation ?? ""
        };
    }
}
