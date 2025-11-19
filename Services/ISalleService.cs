using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public interface ISalleService
    {
        Task<IEnumerable<Salle>> GetAllSallesAsync();
        Task<Salle> GetSalleByIdAsync(int id);
        Task<Salle> AddSalleAsync(Salle salle);
        Task<Salle> UpdateSalleAsync(int id, Salle salle);
        Task<bool> DeleteSalleAsync(int id);
    }

}
