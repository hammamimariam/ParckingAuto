using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Models;

namespace ParckingAuto.Repositories
{
    public class MouvementRepository : IRepository<Mouvement>
    {
        private readonly ParcAutoContext _context;
        public MouvementRepository(ParcAutoContext context) => _context = context;

        public async Task<List<Mouvement>> GetAllAsync() =>
            await _context.Mouvements.Include(m => m.Vehicule).Include(m => m.Chauffeur).ToListAsync();

        public async Task<Mouvement?> GetByIdAsync(int id) => await _context.Mouvements.FindAsync(id);

        public async Task<Mouvement> AddAsync(Mouvement m)
        {
            _context.Mouvements.Add(m);
            await _context.SaveChangesAsync();
            return m;
        }

        public async Task UpdateAsync(Mouvement m)
        {
            _context.Entry(m).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var m = await _context.Mouvements.FindAsync(id);
            if (m != null)
            {
                _context.Mouvements.Remove(m);
                await _context.SaveChangesAsync();
            }
        }
    }
}
