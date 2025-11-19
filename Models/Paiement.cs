namespace ProjetBrima.Models
{
    public class Paiement
    {
        public int Id { get; set; }
        public int EleveId { get; set; }  // Lien avec l'élève
        public decimal Montant { get; set; }  // Montant payé
        public DateTime? DatePaiement { get; set; }
        public bool EstPayé { get; set; }  // Indique si l'élève a payé ou non

        public Eleve Eleve { get; set; }  // Lien avec l'élève
    }

}
