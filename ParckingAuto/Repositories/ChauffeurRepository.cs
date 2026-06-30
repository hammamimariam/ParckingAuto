using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Models;

namespace ParckingAuto.Repositories
{
    public class ChauffeurRepository : IRepository<Chauffeur>
    {
        private readonly ParcAutoContext _context;
        public ChauffeurRepository(ParcAutoContext context) => _context = context;

        public async Task<List<Chauffeur>> GetAllAsync() =>
            await _context.Chauffeurs
                .OrderBy(c => c.Nom)
                .ThenBy(c => c.Prenom)
                .ToListAsync();
        public async Task<Chauffeur?> GetByIdAsync(int id) => await _context.Chauffeurs.FindAsync(id);

        public async Task<Chauffeur> AddAsync(Chauffeur c)
        {
            if (c.UtilisateurId is null or <= 0)
                c.UtilisateurId = null;

            _context.Chauffeurs.Add(c);
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task UpdateAsync(Chauffeur c)
        {
            _context.Entry(c).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var c = await _context.Chauffeurs.FindAsync(id);
            if (c != null)
            {
                _context.Chauffeurs.Remove(c);
                await _context.SaveChangesAsync();
            }
        }
    }
}
