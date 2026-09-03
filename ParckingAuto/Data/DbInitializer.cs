using Microsoft.EntityFrameworkCore;
using ParckingAuto.Data;
using ParckingAuto.Enums;
using ParckingAuto.Models;
using ParckingAuto.Services;
using ParckingAuto.Seed;

public static class DbInitializer
{
    public static void Seed(ParcAutoContext context)
    {
        // Ensure database is migrated
        context.Database.Migrate();

        // 1. Create Admin user if not exists
        if (!context.Utilisateurs.Any(u => u.Email == "admin@parc.com"))
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

        // 2. Create Gestionnaire user
        if (!context.Utilisateurs.Any(u => u.Email == "gestionnaire@parc.com"))
        {
            context.Utilisateurs.Add(new Utilisateur
            {
                Nom = "Gestionnaire",
                Email = "gestionnaire@parc.com",
                MotDePasse = PasswordHasher.Hash("Parc@0"),
                Role = RoleEnum.Gestionnaire
            });
            context.SaveChanges();
        }

        // 3. Ensure Chauffeur users and Chauffeurs
        if (!context.Chauffeurs.Any())
        {
            var chauffeurUser1 = context.Utilisateurs.FirstOrDefault(u => u.Email == "chauffeur@parc.com");
            if (chauffeurUser1 == null)
            {
                chauffeurUser1 = new Utilisateur
                {
                    Nom = "Ben Ali",
                    Email = "chauffeur@parc.com",
                    MotDePasse = PasswordHasher.Hash("Parc@0"),
                    Role = RoleEnum.Chauffeur
                };
                context.Utilisateurs.Add(chauffeurUser1);
                context.SaveChanges();
            }

            var chauffeurUser2 = context.Utilisateurs.FirstOrDefault(u => u.Email == "sami.trabelsi@parc.com");
            if (chauffeurUser2 == null)
            {
                chauffeurUser2 = new Utilisateur
                {
                    Nom = "Sami Trabelsi",
                    Email = "sami.trabelsi@parc.com",
                    MotDePasse = PasswordHasher.Hash("Parc@0"),
                    Role = RoleEnum.Chauffeur
                };
                context.Utilisateurs.Add(chauffeurUser2);
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
                    UtilisateurId = chauffeurUser1.Id
                },
                new Chauffeur
                {
                    Nom = "Trabelsi",
                    Prenom = "Sami",
                    Telephone = "+216 22 987 654",
                    PermisNumero = "23/654321",
                    PermisExpiration = DateTime.Today.AddYears(1),
                    Statut = StatutChauffeurEnum.Disponible,
                    UtilisateurId = chauffeurUser2.Id
                }
            );
            context.SaveChanges();
        }

        // 4. Create Paramètres if not exists
        if (!context.Parametres.Any())
        {
            context.Parametres.Add(new Parametres());
            context.SaveChanges();
        }

        // 5. Create sample vehicles
        if (!context.Vehicules.Any())
        {
            SeedSampleData(context);
        }

        // Cleanup and fix tasks
        CleanupGhostVehicles(context);
        FixOrphanMaintenances(context);
        LinkOrphanChauffeurs(context);
        FixDernierKmVidange(context);
    }

    // Reset the entire database and re-seed from scratch!
    public static void ResetAndSeed(ParcAutoContext context)
    {
        // Delete all data in reverse order of dependencies to avoid foreign key issues!
        context.Alertes.RemoveRange(context.Alertes);
        context.Mouvements.RemoveRange(context.Mouvements);
        context.Maintenances.RemoveRange(context.Maintenances);
        context.Carburants.RemoveRange(context.Carburants);
        context.Vehicules.RemoveRange(context.Vehicules);
        context.Chauffeurs.RemoveRange(context.Chauffeurs);
        context.Utilisateurs.RemoveRange(context.Utilisateurs.Where(u => u.Email != "admin@parc.com")); // Keep admin
        context.Parametres.RemoveRange(context.Parametres);
        context.SaveChanges();

        // Now re-seed everything!
        Seed(context);
        SeedSampleData(context);
    }

    // Helper method to seed all the sample data
    private static void SeedSampleData(ParcAutoContext context)
    {
        // First, ensure we have all the necessary users/chauffeurs
        if (!context.Chauffeurs.Any())
        {
            // Re-run the chauffeur creation part
            var chauffeurUser1 = context.Utilisateurs.FirstOrDefault(u => u.Email == "chauffeur@parc.com");
            if (chauffeurUser1 == null)
            {
                chauffeurUser1 = new Utilisateur
                {
                    Nom = "Ben Ali",
                    Email = "chauffeur@parc.com",
                    MotDePasse = PasswordHasher.Hash("Parc@0"),
                    Role = RoleEnum.Chauffeur
                };
                context.Utilisateurs.Add(chauffeurUser1);
                context.SaveChanges();
            }

            var chauffeurUser2 = context.Utilisateurs.FirstOrDefault(u => u.Email == "sami.trabelsi@parc.com");
            if (chauffeurUser2 == null)
            {
                chauffeurUser2 = new Utilisateur
                {
                    Nom = "Sami Trabelsi",
                    Email = "sami.trabelsi@parc.com",
                    MotDePasse = PasswordHasher.Hash("Parc@0"),
                    Role = RoleEnum.Chauffeur
                };
                context.Utilisateurs.Add(chauffeurUser2);
                context.SaveChanges();
            }

            if (!context.Chauffeurs.Any())
            {
                context.Chauffeurs.AddRange(
                    new Chauffeur
                    {
                        Nom = "Ben Ali",
                        Prenom = "Mohamed",
                        Telephone = "+216 20 123 456",
                        PermisNumero = "24/123456",
                        PermisExpiration = DateTime.Today.AddYears(2),
                        Statut = StatutChauffeurEnum.Disponible,
                        UtilisateurId = chauffeurUser1.Id
                    },
                    new Chauffeur
                    {
                        Nom = "Trabelsi",
                        Prenom = "Sami",
                        Telephone = "+216 22 987 654",
                        PermisNumero = "23/654321",
                        PermisExpiration = DateTime.Today.AddYears(1),
                        Statut = StatutChauffeurEnum.Disponible,
                        UtilisateurId = chauffeurUser2.Id
                    }
                );
                context.SaveChanges();
            }
        }

        // Now add vehicles!
        var vehicles = SampleData.GetSampleVehicles();
        context.Vehicules.AddRange(vehicles);
        context.SaveChanges();

        var chauffeurs = context.Chauffeurs.ToList();

        // Add carburant records
        var carburants = SampleData.GetSampleCarburants(vehicles);
        context.Carburants.AddRange(carburants);
        context.SaveChanges();

        // Add maintenance records
        var maintenances = SampleData.GetSampleMaintenances(vehicles);
        context.Maintenances.AddRange(maintenances);
        context.SaveChanges();

        // Add mouvement records
        var mouvements = SampleData.GetSampleMouvements(vehicles, chauffeurs);
        var ongoingMvt = mouvements.FirstOrDefault(m => m.DateRetour == null);
        if (ongoingMvt != null)
        {
            var assignedChauffeur = chauffeurs.First(c => c.Id == ongoingMvt.ChauffeurId);
            assignedChauffeur.Statut = StatutChauffeurEnum.EnMission;
        }
        context.Mouvements.AddRange(mouvements);
        context.SaveChanges();

        // Add alertes
        var alertes = SampleData.GetSampleAlertes(vehicles);
        context.Alertes.AddRange(alertes);
        context.SaveChanges();
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
