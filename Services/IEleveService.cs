using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public interface IEleveService
    {
        Task<Eleve> GetEleveByIdAsync(int id);
        Task<IEnumerable<Eleve>> GetAllElevesAsync();
        Task<List<Eleve>> GetElevesParClasseAsync(string classe);
        Task DeleteEleveAsync(int id);
        Task CreateEleveAsync(Eleve eleve, IFormFile bulletinFile);
        Task UpdateEleveAsync(Eleve eleve, IFormFile bulletinFile);
        Task<bool> EstPayéParStripeAsync(int eleveId);
        Task<IEnumerable<Eleve>> GetElevesAyantPayéAsync();

    }
}
