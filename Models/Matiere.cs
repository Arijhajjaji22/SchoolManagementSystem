namespace ProjetBrima.Models
{
    public class Matiere
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int Horaire { get; set; } // en heures/semaine
        public string? Classe { get; set; }
        public byte[]? ProgrammePdf { get; set; } // contenu du fichier PDF
    }


}
