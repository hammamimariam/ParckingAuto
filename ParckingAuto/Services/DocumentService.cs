

using ParckingAuto.Models;
using ParckingAuto.Repositories;

namespace ParckingAuto.Services
{
    public class DocumentService
    {
        private readonly DocumentRepository _repo;
        public DocumentService(DocumentRepository repo) => _repo = repo;

        public Task<List<Document>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Document?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<Document> AddAsync(Document d) => _repo.AddAsync(d);
        public Task UpdateAsync(Document d) => _repo.UpdateAsync(d);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
