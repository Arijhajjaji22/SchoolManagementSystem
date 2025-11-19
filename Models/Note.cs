namespace ProjetBrima.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Classe { get; set; }
        public string Matiere { get; set; }

        public int EleveId { get; set; }       // Clé étrangère
        public Eleve? Eleve { get; set; }       // Propriété de navigation
                                                // Ajout des propriétés pour stocker le nom et prénom de l'élève
        public string? NomEleve { get; set; }   // Nom de l'élève
        public string? PrenomEleve { get; set; } // Prénom de l'élève

        public float Valeur { get; set; }
    }

}
