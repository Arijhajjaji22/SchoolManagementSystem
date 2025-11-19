using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public interface IAbsenceService
    {
        Task EnregistrerAbsenceAsync(int eleveId, int matiereId, string status);
        Task<List<Absence>> GetAbsencesByEleveAsync(int eleveId);
        Task<Absence?> GetDernierStatutAsync(int eleveId);
    }

}
