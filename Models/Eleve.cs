namespace ProjetBrima.Models
{
    public class Eleve
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Classe { get; set; }
        public string EcolePrecedente { get; set; }
        public byte[]? BulletinFile { get; set; } // Stocke le contenu du fichier PDF
        public string? Filiere { get; set; }
        public string? Option { get; set; } // Nouveau champ : Option au lieu de LangueOption
        public string Email { get; set; } // Nouveau champ : Email de l'Élève
        public string EmailParent { get; set; } // Nouveau champ : Email du Parent
        public DateTime DateDeNaissance { get; set; } // Nouveau champ : Date de Naissance
        public string LieuDeNaissance { get; set; } // Nouveau champ : Lieu de Naissance
        public string Nationalite { get; set; } // Nouveau champ : Nationalité
        public string? EmailInstitutionnel { get; set; } // Email généré
        public string? MotDePasseInstitutionnel { get; set; } // Mot de passe généré
        public string? Genre { get; set; } // Homme ou Femme

    }


}
