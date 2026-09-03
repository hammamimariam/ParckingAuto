using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Models;

namespace ParckingAuto.Repositories
{
    public class AuditRepository : IRepository<Audit>
    {
        private readonly ParcAutoContext _context;
        public AuditRepository(ParcAutoContext context) => _context = context;

        public async Task<List<Audit>> GetAllAsync() => await _context.Audits.Include(a => a.Utilisateur).OrderByDescending(a => a.Timestamp).ToListAsync();
        public async Task<Audit?> GetByIdAsync(int id) => await _context.Audits.Include(a => a.Utilisateur).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<Audit> AddAsync(Audit a)
        {
            _context.Audits.Add(a);
            await _context.SaveChangesAsync();
            return a;
        }

        public async Task UpdateAsync(Audit a)
        {
            _context.Entry(a).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var a = await _context.Audits.FindAsync(id);
            if (a != null)
            {
                _context.Audits.Remove(a);
                await _context.SaveChangesAsync();
            }
        }
    }
}
