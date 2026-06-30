using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Enums;
using ParckingAuto.Models;
using ParckingAuto.Services;

public static class DbInitializer
{
    public static void Seed(ParcAutoContext context)
    {
        if (!context.Utilisateurs.Any())
        {
            context.Utilisateurs.Add(new Utilisateur
            {
                Nom = "Admin",
                Email = "admin@parc.com",
                MotDePasse = PasswordHasher.Hash("Parc@0"),
                Role = RoleEnum.Administrateur
            });
            context.SaveChanges();
        }
        else
        {
            var admin = context.Utilisateurs.FirstOrDefault(u => u.Email == "admin@parc.com");
            if (admin != null && !admin.MotDePasse.StartsWith("$2"))
            {
                admin.MotDePasse = PasswordHasher.Hash("Parc@0");
                context.SaveChanges();
            }
        }

        if (!context.Parametres.Any())
        {
            context.Parametres.Add(new Parametres());
            context.SaveChanges();
        }

        if (!context.Chauffeurs.Any())
        {
            var chauffeurUser = context.Utilisateurs.FirstOrDefault(u => u.Role == RoleEnum.Chauffeur);
            if (chauffeurUser == null)
            {
                chauffeurUser = new Utilisateur
                {
                    Nom = "Ben Ali",
                    Email = "chauffeur@parc.com",
                    MotDePasse = PasswordHasher.Hash("Parc@0"),
                    Role = RoleEnum.Chauffeur
                };
                context.Utilisateurs.Add(chauffeurUser);
                context.SaveChanges();
            }

            context.Chauffeurs.AddRange(
                new Chauffeur
                {
                    Nom = "Ben Ali",
                    Prenom = "Mohamed",
                    Telephone = "+216 20 123 456",
                    PermisNumero = "24/123456",
                    PermisExpiration = DateTime.Today.AddYears(2),
                    Statut = StatutChauffeurEnum.Disponible,
                    UtilisateurId = chauffeurUser.Id
                }
            );
            context.SaveChanges();

            if (!context.Utilisateurs.Any(u => u.Email == "sami.trabelsi@parc.com"))
            {
                var samiUser = new Utilisateur
                {
                    Nom = "Sami Trabelsi",
                    Email = "sami.trabelsi@parc.com",
                    MotDePasse = PasswordHasher.Hash("Parc@0"),
                    Role = RoleEnum.Chauffeur
                };
                context.Utilisateurs.Add(samiUser);
                context.SaveChanges();

                context.Chauffeurs.Add(new Chauffeur
                {
                    Nom = "Trabelsi",
                    Prenom = "Sami",
                    Telephone = "+216 22 987 654",
                    PermisNumero = "23/654321",
                    PermisExpiration = DateTime.Today.AddYears(1),
                    Statut = StatutChauffeurEnum.Disponible,
                    UtilisateurId = samiUser.Id
                });
                context.SaveChanges();
            }
        }

        CleanupGhostVehicles(context);
        FixOrphanMaintenances(context);
        LinkOrphanChauffeurs(context);
        FixDernierKmVidange(context);
    }

    private static void FixDernierKmVidange(ParcAutoContext context)
    {
        var vehicles = context.Vehicules
            .Where(v => v.DernierKmVidange == 0 && v.Kilometrage > 0)
            .ToList();

        foreach (var v in vehicles)
            v.DernierKmVidange = v.Kilometrage;

        if (vehicles.Count > 0)
            context.SaveChanges();
    }

    private static void CleanupGhostVehicles(ParcAutoContext context)
    {
        var ghosts = context.Vehicules
            .Where(v => string.IsNullOrWhiteSpace(v.Immatriculation)
                     && string.IsNullOrWhiteSpace(v.Marque)
                     && string.IsNullOrWhiteSpace(v.Modele))
            .ToList();

        if (ghosts.Count == 0) return;

        foreach (var ghost in ghosts)
        {
            var mouvements = context.Mouvements.Where(m => m.VehiculeId == ghost.Id).ToList();
            foreach (var m in mouvements)
            {
                var chauffeur = context.Chauffeurs.Find(m.ChauffeurId);
                if (chauffeur != null && m.DateRetour == null)
                    chauffeur.Statut = StatutChauffeurEnum.Disponible;
            }
            context.Mouvements.RemoveRange(mouvements);
            context.Vehicules.Remove(ghost);
        }

        context.SaveChanges();
    }

    private static void FixOrphanMaintenances(ParcAutoContext context)
    {
        var orphans = context.Maintenances
            .Include(m => m.Vehicule)
            .Where(m => m.Vehicule != null && string.IsNullOrWhiteSpace(m.Vehicule.Immatriculation))
            .ToList();

        foreach (var maintenance in orphans)
        {
            var candidate = context.Vehicules
                .Where(v => !string.IsNullOrWhiteSpace(v.Immatriculation))
                .OrderBy(v => Math.Abs(v.Kilometrage - maintenance.KilometrageIntervention))
                .FirstOrDefault();

            if (candidate != null)
                maintenance.VehiculeId = candidate.Id;
        }

        if (orphans.Count > 0)
            context.SaveChanges();
    }

    private static void LinkOrphanChauffeurs(ParcAutoContext context)
    {
        var orphans = context.Chauffeurs.Where(c => c.UtilisateurId == null).ToList();
        foreach (var chauffeur in orphans)
        {
            var baseEmail = $"{chauffeur.Prenom}.{chauffeur.Nom}".ToLowerInvariant().Replace(" ", "");
            var email = $"{baseEmail}@parc.com";
            var suffix = 1;
            while (context.Utilisateurs.Any(u => u.Email == email))
                email = $"{baseEmail}{suffix++}@parc.com";

            var user = new Utilisateur
            {
                Nom = $"{chauffeur.Prenom} {chauffeur.Nom}".Trim(),
                Email = email,
                MotDePasse = PasswordHasher.Hash("Parc@0"),
                Role = RoleEnum.Chauffeur
            };
            context.Utilisateurs.Add(user);
            context.SaveChanges();
            chauffeur.UtilisateurId = user.Id;
        }

        if (orphans.Count > 0)
            context.SaveChanges();
    }
}
