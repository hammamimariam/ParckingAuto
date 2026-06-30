using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Enums;
using ParckingAuto.Models;
using ParckingAuto.Repositories;

namespace ParckingAuto.Services
{
    public class MouvementService
    {
        private readonly MouvementRepository _repo;
        private readonly ParcAutoContext _context;

        public MouvementService(MouvementRepository repo, ParcAutoContext context)
        {
            _repo = repo;
            _context = context;
        }

        public Task<List<Mouvement>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Mouvement?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<Mouvement> AddAsync(Mouvement m)
        {
            if (m.VehiculeId <= 0)
                throw new InvalidOperationException("Véhicule invalide.");

            if (m.ChauffeurId <= 0)
                throw new InvalidOperationException("Chauffeur invalide.");

            var vehicule = await _context.Vehicules.FindAsync(m.VehiculeId)
                ?? throw new InvalidOperationException("Véhicule introuvable.");

            var vehiculeEnMission = await _context.Mouvements
                .AnyAsync(x => x.VehiculeId == m.VehiculeId && x.DateRetour == null);

            if (vehiculeEnMission)
                throw new InvalidOperationException("Ce véhicule est déjà en mission.");

            m.Vehicule = null;
            m.Chauffeur = null;

            var chauffeur = await _context.Chauffeurs.FindAsync(m.ChauffeurId);
            if (chauffeur != null)
                chauffeur.Statut = StatutChauffeurEnum.EnMission;

            var created = await _repo.AddAsync(m);
            await _context.SaveChangesAsync();
            return created;
        }

        public async Task UpdateAsync(Mouvement m)
        {
            var existing = await _context.Mouvements
                .Include(x => x.Vehicule)
                .Include(x => x.Chauffeur)
                .FirstOrDefaultAsync(x => x.Id == m.Id);

            if (existing == null)
                throw new InvalidOperationException("Mouvement introuvable.");

            existing.DateRetour = m.DateRetour;
            existing.KmRetour = m.KmRetour;
            existing.Destination = m.Destination;

            if (m.DateRetour.HasValue && m.KmRetour.HasValue)
            {
                if (m.KmRetour.Value < existing.KmDepart)
                    throw new InvalidOperationException("Le kilométrage de retour ne peut pas être inférieur au départ.");

                if (existing.Vehicule != null && m.KmRetour.Value > existing.Vehicule.Kilometrage)
                    existing.Vehicule.Kilometrage = m.KmRetour.Value;

                if (existing.Chauffeur != null)
                    existing.Chauffeur.Statut = StatutChauffeurEnum.Disponible;
            }

            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
