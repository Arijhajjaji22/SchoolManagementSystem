namespace ProjetBrima.Models
{
    public class EmploiDuTemps
    {
        public int Id { get; set; }
        public int ProfesseurId { get; set; }
        public int MatiereId { get; set; }
        public string Classe { get; set; }
        public int SalleId { get; set; }
        public DateTime DateDebutSemaine { get; set; }
        public string Horaire { get; set; } // exemple : "Lundi 08h00-10h00"
        public string? NumSalle { get; set; }
        public Professeur Professeur { get; set; }
        public Matiere Matiere { get; set; }
        public Salle Salle { get; set; }
        public byte[]? PdfEmploi { get; set; } // ← optionnel pour stocker le fichier PDF

    }
}
