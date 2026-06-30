using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Models;

namespace ParckingAuto.Repositories
{
    public class AlerteRepository : IRepository<Alerte>
    {
        private readonly ParcAutoContext _context;
        public AlerteRepository(ParcAutoContext context) => _context = context;

        public async Task<List<Alerte>> GetAllAsync() => await _context.Alertes.Include(a => a.Vehicule).ToListAsync();
        public async Task<Alerte?> GetByIdAsync(int id) => await _context.Alertes.FindAsync(id);

        public async Task<Alerte> AddAsync(Alerte a)
        {
            _context.Alertes.Add(a);
            await _context.SaveChangesAsync();
            return a;
        }

        public async Task UpdateAsync(Alerte a)
        {
            _context.Entry(a).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var a = await _context.Alertes.FindAsync(id);
            if (a != null)
            {
                _context.Alertes.Remove(a);
                await _context.SaveChangesAsync();
            }
        }
    }
}
