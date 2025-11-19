using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public interface IMatiereService
    {
        IEnumerable<Matiere> GetMatieresByClasse(string classe);
        Matiere GetMatiereById(int id);
        void AddMatiere(Matiere matiere);
        void UpdateMatiere(Matiere matiere);
        void DeleteMatiere(int id);
        IEnumerable<Matiere> GetAllMatieres();


    }

}
