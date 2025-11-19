using System.ComponentModel.DataAnnotations;

namespace ProjetBrima.Models
{
    public class ComptesPersonnel
    {
        [Key]
        public int Id { get; set; }  // Id auto-incrémenté pour chaque directeur
        public string Prenom { get; set; }
        public string Nom { get; set; }
        public string NumeroCIN { get; set; }
        public string EmailPersonnel { get; set; }
        public string Telephone { get; set; }
        public string Adresse { get; set; }
        public string EmailInstitutionnel { get; set; }
        public string? MotDePasse { get; set; }

        // Nouveau champ pour spécifier le rôle (ex : Directeur, Secrétaire administratif, Surveillant général)
        public string? Role { get; set; }

    }
}
