namespace ProjetBrima.Models
{
    public class Absence
    {
        public int Id { get; set; }
        public int EleveId { get; set; }    // Clé étrangère vers l'élève
        public int MatiereId { get; set; }   // Clé étrangère vers la matière
        public string Status { get; set; }   // "Présent" ou "Absent"
        public Eleve Eleve { get; set; }     // Navigation vers l'entité Eleve
        public Matiere Matiere { get; set; } // Navigation vers l'entité Matiere
    }

}
