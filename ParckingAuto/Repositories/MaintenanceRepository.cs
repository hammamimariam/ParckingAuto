using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Models;

namespace ParckingAuto.Repositories
{
    public class MaintenanceRepository : IRepository<ParckingAuto.Models.Maintenance>
    {
        private readonly ParcAutoContext _context;
        public MaintenanceRepository(ParcAutoContext context) => _context = context;

            public async Task<List<ParckingAuto.Models.Maintenance>> GetAllAsync() => await _context.Maintenances.Include(m => m.Vehicule).ToListAsync();
        public async Task<ParckingAuto.Models.Maintenance?> GetByIdAsync(int id) =>
            await _context.Maintenances.Include(m => m.Vehicule).FirstOrDefaultAsync(m => m.Id == id);

        public async Task<ParckingAuto.Models.Maintenance> AddAsync(ParckingAuto.Models.Maintenance m)
        {
            m.Vehicule = null;
            _context.Maintenances.Add(m);
            await _context.SaveChangesAsync();
            return m;
        }

        public async Task UpdateAsync(ParckingAuto.Models.Maintenance m)
        {
            m.Vehicule = null;
            _context.Entry(m).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var m = await _context.Maintenances.FindAsync(id);
            if (m != null)
            {
                _context.Maintenances.Remove(m);
                await _context.SaveChangesAsync();
            }
        }
    }
}
