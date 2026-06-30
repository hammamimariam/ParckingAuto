using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Models;

namespace ParckingAuto.Repositories
{
    public class UtilisateurRepository : IRepository<Utilisateur>
    {
        private readonly ParcAutoContext _context;
        public UtilisateurRepository(ParcAutoContext context) => _context = context;

        public async Task<List<Utilisateur>> GetAllAsync() => await _context.Utilisateurs.ToListAsync();
        public async Task<Utilisateur?> GetByIdAsync(int id) => await _context.Utilisateurs.FindAsync(id);

        public async Task<Utilisateur> AddAsync(Utilisateur u)
        {
            _context.Utilisateurs.Add(u);
            await _context.SaveChangesAsync();
            return u;
        }

        public async Task UpdateAsync(Utilisateur u)
        {
            _context.Entry(u).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var u = await _context.Utilisateurs.FindAsync(id);
            if (u != null)
            {
                _context.Utilisateurs.Remove(u);
                await _context.SaveChangesAsync();
            }
        }
    }
}
