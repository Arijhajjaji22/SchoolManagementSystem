using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public interface IDirecteurService
    {
        Task<List<Directeur>> GetAllDirecteursAsync();
        Task<Directeur> GetDirecteurByIdAsync(int id);
        Task AddDirecteurAsync(Directeur directeur);
        Task UpdateDirecteurAsync(Directeur directeur);
        Task DeleteDirecteurAsync(int id);
    }

}
