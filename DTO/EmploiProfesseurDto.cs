namespace ProjetBrima.DTO
{
    public class EmploiProfesseurDto
    {
        public int Id { get; set; }
        public string ProfesseurNom { get; set; }
        public string ProfesseurPrenom { get; set; }
        public string Classe { get; set; }
        public string NumSalle { get; set; }
        public DateTime DateAjout { get; set; }
        public string PdfBase64 { get; set; }
    }

}
