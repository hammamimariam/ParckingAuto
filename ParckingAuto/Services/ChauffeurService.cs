
using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Enums;
using ParckingAuto.Models;
using ParckingAuto.Repositories;

namespace ParckingAuto.Services
{
    public class ChauffeurService
    {
        private readonly ChauffeurRepository _repo;
        private readonly ParcAutoContext _context;

        public ChauffeurService(ChauffeurRepository repo, ParcAutoContext context)
        {
            _repo = repo;
            _context = context;
        }

        public Task<List<Chauffeur>> GetAllAsync() =>
            _context.Chauffeurs
                .Include(c => c.Utilisateur)
                .OrderBy(c => c.Nom)
                .ThenBy(c => c.Prenom)
                .ToListAsync();

        public Task<Chauffeur?> GetByIdAsync(int id) =>
            _context.Chauffeurs
                .Include(c => c.Utilisateur)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Chauffeur> AddAsync(Chauffeur c, string email, string motDePasse)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidOperationException("L'email est requis pour créer le compte chauffeur.");

            if (string.IsNullOrWhiteSpace(motDePasse))
                throw new InvalidOperationException("Le mot de passe est requis pour créer le compte chauffeur.");

            email = email.Trim().ToLowerInvariant();

            if (await _context.Utilisateurs.AnyAsync(u => u.Email == email))
                throw new InvalidOperationException("Cet email est déjà utilisé par un autre compte.");

            if (c.UtilisateurId is > 0)
                await EnsureUtilisateurLinkAsync(c);

            if (c.UtilisateurId is null or <= 0)
            {
                var user = new Utilisateur
                {
                    Nom = $"{c.Prenom} {c.Nom}".Trim(),
                    Email = email,
                    MotDePasse = PasswordHasher.Hash(motDePasse),
                    Role = RoleEnum.Chauffeur
                };
                _context.Utilisateurs.Add(user);
                await _context.SaveChangesAsync();
                c.UtilisateurId = user.Id;
            }

            c.Utilisateur = null;
            return await _repo.AddAsync(c);
        }

        public async Task UpdateAsync(Chauffeur c, string email, string? motDePasse)
        {
            var existing = await _context.Chauffeurs
                .Include(x => x.Utilisateur)
                .FirstOrDefaultAsync(x => x.Id == c.Id)
                ?? throw new InvalidOperationException("Chauffeur introuvable.");

            existing.Nom = c.Nom;
            existing.Prenom = c.Prenom;
            existing.Telephone = c.Telephone;
            existing.PermisNumero = c.PermisNumero;
            existing.PermisExpiration = c.PermisExpiration;
            existing.Statut = c.Statut;

            if (existing.Utilisateur != null)
            {
                if (!string.IsNullOrWhiteSpace(email))
                {
                    email = email.Trim().ToLowerInvariant();
                    if (await _context.Utilisateurs.AnyAsync(u => u.Email == email && u.Id != existing.Utilisateur.Id))
                        throw new InvalidOperationException("Cet email est déjà utilisé par un autre compte.");

                    existing.Utilisateur.Email = email;
                }

                existing.Utilisateur.Nom = $"{existing.Prenom} {existing.Nom}".Trim();
                existing.Utilisateur.Role = RoleEnum.Chauffeur;

                if (!string.IsNullOrWhiteSpace(motDePasse))
                    existing.Utilisateur.MotDePasse = PasswordHasher.Hash(motDePasse);
            }
            else if (!string.IsNullOrWhiteSpace(email))
            {
                email = email.Trim().ToLowerInvariant();
                if (await _context.Utilisateurs.AnyAsync(u => u.Email == email))
                    throw new InvalidOperationException("Cet email est déjà utilisé par un autre compte.");

                var password = string.IsNullOrWhiteSpace(motDePasse) ? "Parc@0" : motDePasse;
                var user = new Utilisateur
                {
                    Nom = $"{existing.Prenom} {existing.Nom}".Trim(),
                    Email = email,
                    MotDePasse = PasswordHasher.Hash(password),
                    Role = RoleEnum.Chauffeur
                };
                _context.Utilisateurs.Add(user);
                await _context.SaveChangesAsync();
                existing.UtilisateurId = user.Id;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var chauffeur = await _context.Chauffeurs
                .Include(c => c.Utilisateur)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (chauffeur == null) return;

            var userId = chauffeur.UtilisateurId;
            var user = chauffeur.Utilisateur;

            _context.Chauffeurs.Remove(chauffeur);
            await _context.SaveChangesAsync();

            if (user != null && user.Role == RoleEnum.Chauffeur)
            {
                var stillLinked = await _context.Chauffeurs.AnyAsync(c => c.UtilisateurId == userId);
                if (!stillLinked)
                {
                    _context.Utilisateurs.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private async Task EnsureUtilisateurLinkAsync(Chauffeur c)
        {
            var user = await _context.Utilisateurs.FindAsync(c.UtilisateurId)
                ?? throw new InvalidOperationException("Le compte utilisateur associé est introuvable.");

            if (user.Role != RoleEnum.Chauffeur)
                throw new InvalidOperationException("Seul un compte avec le rôle Chauffeur peut être associé.");

            if (await _context.Chauffeurs.AnyAsync(x => x.UtilisateurId == c.UtilisateurId && x.Id != c.Id))
                throw new InvalidOperationException("Ce compte utilisateur est déjà associé à un autre chauffeur.");
        }
    }
}
