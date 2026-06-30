
using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Enums;
using ParckingAuto.Models;
using ParckingAuto.Repositories;

namespace ParckingAuto.Services
{
    public class MaintenanceService
    {
        private readonly MaintenanceRepository _repo;
        private readonly ParcAutoContext _context;

        public MaintenanceService(MaintenanceRepository repo, ParcAutoContext context)
        {
            _repo = repo;
            _context = context;
        }

        public Task<List<Models.Maintenance>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Models.Maintenance?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<Models.Maintenance> AddAsync(Models.Maintenance m)
        {
            var created = await _repo.AddAsync(m);
            await ApplyVidangeSideEffectsAsync(created);
            return created;
        }

        public async Task UpdateAsync(Models.Maintenance m)
        {
            await _repo.UpdateAsync(m);
            await ApplyVidangeSideEffectsAsync(m);
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        private async Task ApplyVidangeSideEffectsAsync(Models.Maintenance m)
        {
            if (string.IsNullOrWhiteSpace(m.TypeIntervention) ||
                !m.TypeIntervention.Contains("vidange", StringComparison.OrdinalIgnoreCase))
                return;

            var vehicule = await _context.Vehicules.FindAsync(m.VehiculeId);
            if (vehicule == null) return;

            var km = m.KilometrageIntervention > 0 ? m.KilometrageIntervention : vehicule.Kilometrage;
            vehicule.DernierKmVidange = km;

            var openAlerts = await _context.Alertes
                .Where(a => a.VehiculeId == m.VehiculeId
                         && a.TypeAlerte == TypeAlerteEnum.Vidange
                         && a.Statut != StatutAlerteEnum.Resolue)
                .ToListAsync();

            foreach (var alerte in openAlerts)
            {
                alerte.Statut = StatutAlerteEnum.Resolue;
                alerte.DateResolution = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }
    }
}
