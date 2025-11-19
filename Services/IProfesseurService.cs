using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public interface IProfesseurService
    {
        Task<List<Professeur>> GetAllProfesseursAsync();
        Task<Professeur> GetProfesseurByIdAsync(int id);
        Task AddProfesseurAsync(Professeur Professeur);
        Task UpdateProfesseurAsync(Professeur Professeur);
        Task DeleteProfesseurAsync(int id);
    }
}
