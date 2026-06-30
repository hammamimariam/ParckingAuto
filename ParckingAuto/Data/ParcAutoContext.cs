using Microsoft.EntityFrameworkCore;
using ParckingAuto.Models;

namespace ParckingAuto.Data
{
    public class ParcAutoContext : DbContext
    {
        public ParcAutoContext(DbContextOptions<ParcAutoContext> options) : base(options) { }

        // Tables
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Vehicule> Vehicules { get; set; }
        public DbSet<Chauffeur> Chauffeurs { get; set; }
        public DbSet<Mouvement> Mouvements { get; set; }
        public DbSet<Carburant> Carburants { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Alerte> Alertes { get; set; }
        public DbSet<Parametres> Parametres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relations
            modelBuilder.Entity<Chauffeur>()
                .HasOne(c => c.Utilisateur)
                .WithMany()
                .HasForeignKey(c => c.UtilisateurId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            modelBuilder.Entity<Mouvement>()
                .HasOne(m => m.Vehicule)
                .WithMany()
                .HasForeignKey(m => m.VehiculeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mouvement>()
                .HasOne(m => m.Chauffeur)
                .WithMany()
                .HasForeignKey(m => m.ChauffeurId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Carburant>()
                .HasOne(c => c.Vehicule)
                .WithMany()
                .HasForeignKey(c => c.VehiculeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.Vehicule)
                .WithMany()
                .HasForeignKey(m => m.VehiculeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Vehicule)
                .WithMany()
                .HasForeignKey(d => d.VehiculeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Alerte>()
                .HasOne(a => a.Vehicule)
                .WithMany()
                .HasForeignKey(a => a.VehiculeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Enum conversions pour stockage en base
            modelBuilder.Entity<Utilisateur>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Vehicule>()
                .Property(v => v.TypeCarburant)
                .HasConversion<string>();

            modelBuilder.Entity<Document>()
                .Property(d => d.TypeDocument)
                .HasConversion<string>();

            modelBuilder.Entity<Alerte>()
                .Property(a => a.TypeAlerte)
                .HasConversion<string>();

            modelBuilder.Entity<Alerte>()
                .Property(a => a.Statut)
                .HasConversion<string>();

            modelBuilder.Entity<Chauffeur>()
                .Property(c => c.Statut)
                .HasConversion<string>();
        }
    }
}
