namespace ProjetBrima.Models
{
    public class EmploisDuTempsPDF
    {
        public int Id { get; set; }

        public int ProfesseurId { get; set; }
        public string ProfesseurNom { get; set; }
        public string ProfesseurPrenom { get; set; }
        public string Classe { get; set; }
        public int SalleId { get; set; }
        public string NumSalle { get; set; }

        public DateTime DateAjout { get; set; } = DateTime.Now;

        public string PdfBase64 { get; set; }
    }

}
