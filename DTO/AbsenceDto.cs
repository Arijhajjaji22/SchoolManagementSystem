namespace ProjetBrima.DTO
{
    public class AbsenceDto
    {
        public int EleveId { get; set; }
        public int MatiereId { get; set; }
        public string Status { get; set; }  // "Présent" ou "Absent"
    }

}
