using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Models;

namespace ParckingAuto.Repositories
{
    public class VehiculeRepository : IRepository<Vehicule>
    {
        private readonly ParcAutoContext _context;
        public VehiculeRepository(ParcAutoContext context) => _context = context;

        public async Task<List<Vehicule>> GetAllAsync() => await _context.Vehicules.ToListAsync();
        public async Task<Vehicule?> GetByIdAsync(int id) => await _context.Vehicules.FindAsync(id);

        public async Task<Vehicule> AddAsync(Vehicule v)
        {
            _context.Vehicules.Add(v);
            await _context.SaveChangesAsync();
            return v;
        }

        public async Task UpdateAsync(Vehicule v)
        {
            _context.Entry(v).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var v = await _context.Vehicules.FindAsync(id);
            if (v != null)
            {
                _context.Vehicules.Remove(v);
                await _context.SaveChangesAsync();
            }
        }
    }
}
