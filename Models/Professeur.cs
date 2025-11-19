using System.ComponentModel.DataAnnotations;

namespace ProjetBrima.Models
{
    public class Professeur
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
        public string? MotDePasse { get; set; }  // Assurez-vous que les mots de passe sont correctement cryptés avant de les stocker.
        public string Matiere { get; set; }

    }
}
