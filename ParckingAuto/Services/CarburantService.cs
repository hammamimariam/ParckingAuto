using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Models;
using ParckingAuto.Repositories;

namespace ParckingAuto.Services
{
    public class CarburantService
    {
        private readonly CarburantRepository _repo;
        private readonly ParcAutoContext _context;

        public CarburantService(CarburantRepository repo, ParcAutoContext context)
        {
            _repo = repo;
            _context = context;
        }

        public Task<List<Carburant>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Carburant?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<Carburant> AddAsync(Carburant c)
        {
            var vehicule = await _context.Vehicules.FindAsync(c.VehiculeId);
            if (vehicule != null && c.Kilometrage > vehicule.Kilometrage)
                vehicule.Kilometrage = c.Kilometrage;

            var created = await _repo.AddAsync(c);
            await _context.SaveChangesAsync();
            return created;
        }

        public Task UpdateAsync(Carburant c) => _repo.UpdateAsync(c);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public double CalculerConsommationMoyenne(List<Carburant> records)
        {
            if (records.Count < 2) return 0;
            var ordered = records.OrderBy(r => r.Kilometrage).ToList();
            var litres = ordered.Sum(r => (double)r.VolumeLitres);
            var km = ordered.Last().Kilometrage - ordered.First().Kilometrage;
            return km > 0 ? (litres * 100) / km : 0;
        }
    }
}
