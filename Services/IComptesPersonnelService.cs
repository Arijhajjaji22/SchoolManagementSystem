using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public interface IComptesPersonnelService
    {
        Task<List<ComptesPersonnel>> GetAllAsync();
        Task<ComptesPersonnel> GetByIdAsync(int id);
        Task<ComptesPersonnel> AddAsync(ComptesPersonnel personnel);
        Task<ComptesPersonnel> UpdateAsync(int id, ComptesPersonnel personnel);
        Task<bool> DeleteAsync(int id);
    }
}
