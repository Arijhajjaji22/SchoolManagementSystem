using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public interface IEmploiService
    {
        Task<List<EmploiDuTemps>> GenererEmploiAsync(int professeurId, string matiereNom, List<string> classes = null);
        Task<List<EmploiDuTemps>> GetAllAsync();
        Task<bool> SalleEstDisponibleAsync(int salleId, string horaire);
        Task<EmploiDuTemps?> GetByIdAsync(int id);
        Task UpdateAsync(EmploiDuTemps emploi);
        Task<List<EmploisDuTempsPDF>> GetEmploisDuProfAsync();
        Task<List<EmploiDuTemps>> GetCreneauxByEmploiIdAsync(int emploiId);

    }
}
