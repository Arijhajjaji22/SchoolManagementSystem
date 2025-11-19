using Microsoft.EntityFrameworkCore;
using ProjetBrima.Controllers;
using ProjetBrima.Models;

namespace ProjetBrima.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

      
        public DbSet<User> Users { get; set; }
        public DbSet<Directeur> Directeurs { get; set; }
        public DbSet<Professeur> Professeurs { get; set; }
        public DbSet<ComptesPersonnel> ComptesPersonnels { get; set; }
        public DbSet<Eleve> Eleves { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        // Configuration des relations et autres paramètres
        public DbSet<Matiere> Matiere{ get; set; }
        public DbSet<Salle> Salles { get; set; }
        public DbSet<EmploiDuTemps> Emplois { get; set; }
        public DbSet<EmploisDuTempsPDF> EmploisPDF { get; set; }
        public DbSet<Absence> Absence { get; set; }
        public DbSet<DemandeConge> DemandesConge { get; set; }
        public DbSet<Note> Notes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de la relation entre Eleve et Paiement
            modelBuilder.Entity<Paiement>()
                .HasOne(p => p.Eleve)  // Un paiement est lié à un élève
                .WithMany()  // Un élève peut avoir plusieurs paiements
                .HasForeignKey(p => p.EleveId)  // Clé étrangère
                .OnDelete(DeleteBehavior.Cascade);  // Comportement lors de la suppression (optionnel)

            // Vous pouvez ajouter d'autres configurations ici si nécessaire
        }
    }
}
