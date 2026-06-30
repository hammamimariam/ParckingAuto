
using ParckingAuto.Models;
using ParckingAuto.Repositories;

namespace ParckingAuto.Services
{
    public class UtilisateurService
    {
        private readonly UtilisateurRepository _repo;
        public UtilisateurService(UtilisateurRepository repo) => _repo = repo;

        public Task<List<Utilisateur>> GetAllAsync() => _repo.GetAllAsync();
        public Task<Utilisateur?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<Utilisateur> AddAsync(Utilisateur u) => _repo.AddAsync(u);
        public Task UpdateAsync(Utilisateur u) => _repo.UpdateAsync(u);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
