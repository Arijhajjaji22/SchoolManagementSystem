using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public interface IDemandeCongeService
    {
        Task<List<DemandeConge>> GetDemandesAsync();
        Task<DemandeConge> GetDemandeByIdAsync(int id);
        Task<bool> AddDemandeAsync(DemandeConge demande);
        Task<bool> UpdateDemandeStatutAsync(int id, string statut);
        Task<bool> UpdateDemandeAsync(DemandeConge demande);
        Task<bool> DeleteDemandeAsync(int id);

    }

}
