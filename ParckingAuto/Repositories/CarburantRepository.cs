using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Models;

namespace ParckingAuto.Repositories
{
    public class CarburantRepository : IRepository<Carburant>
    {
        private readonly ParcAutoContext _context;
        public CarburantRepository(ParcAutoContext context) => _context = context;

        public async Task<List<Carburant>> GetAllAsync() => await _context.Carburants.Include(c => c.Vehicule).ToListAsync();
        public async Task<Carburant?> GetByIdAsync(int id) => await _context.Carburants.FindAsync(id);

        public async Task<Carburant> AddAsync(Carburant c)
        {
            _context.Carburants.Add(c);
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task UpdateAsync(Carburant c)
        {
            _context.Entry(c).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var c = await _context.Carburants.FindAsync(id);
            if (c != null)
            {
                _context.Carburants.Remove(c);
                await _context.SaveChangesAsync();
            }
        }
    }
}
