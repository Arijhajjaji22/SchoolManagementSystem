namespace ProjetBrima.Models
{
    public class DemandeConge
    {
        public int Id { get; set; }
        public string NomEnseignant { get; set; }
        public string EmailEnseignant { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string Motif { get; set; }
        public string Statut { get; set; } // "En attente", "Approuvé", "Refusé"
    }

}
