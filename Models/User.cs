using System.ComponentModel.DataAnnotations;

namespace ProjetBrima.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } // Admin, Étudiant, Prof, etc.

        public bool IsEmailVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 📸 Photo (URL ou chemin du fichier)
        [StringLength(500)]
        public string? Photo { get; set; }

        // 📍 Localisation
        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        // ☎️ Numéro de téléphone
        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
    }
}
