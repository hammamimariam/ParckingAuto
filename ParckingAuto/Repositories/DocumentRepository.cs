
using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Models;
namespace ParckingAuto.Repositories
{
    public class DocumentRepository : IRepository<Document>
    {
        private readonly ParcAutoContext _context;
        public DocumentRepository(ParcAutoContext context) => _context = context;

        public async Task<List<Document>> GetAllAsync() => await _context.Documents.Include(d => d.Vehicule).ToListAsync();

        public async Task<Document?> GetByIdAsync(int id) => await _context.Documents.FindAsync(id);

        public async Task<Document> AddAsync(Document d)
        {
            _context.Documents.Add(d);
            await _context.SaveChangesAsync();
            return d;
        }

        public async Task UpdateAsync(Document d)
        {
            _context.Entry(d).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var d = await _context.Documents.FindAsync(id);
            if (d != null)
            {
                _context.Documents.Remove(d);
                await _context.SaveChangesAsync();
            }
        }
    }
}
