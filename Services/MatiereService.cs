using ProjetBrima.Data;
using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public class MatiereService : IMatiereService
    {
        private readonly ApplicationDbContext _context;

        public MatiereService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Matiere> GetMatieresByClasse(string classe)
        {
            return _context.Matiere.Where(m => m.Classe == classe).ToList();
        }

        public Matiere GetMatiereById(int id)
        {
            return _context.Matiere.FirstOrDefault(m => m.Id == id);
        }

        public void AddMatiere(Matiere matiere)
        {
            _context.Matiere.Add(matiere);
            _context.SaveChanges();
        }

        public void UpdateMatiere(Matiere matiere)
        {
            _context.Matiere.Update(matiere);
            _context.SaveChanges();
        }

        public void DeleteMatiere(int id)
        {
            var matiere = _context.Matiere.Find(id);
            if (matiere != null)
            {
                _context.Matiere.Remove(matiere);
                _context.SaveChanges();
            }
        }
        public IEnumerable<Matiere> GetAllMatieres()
        {
            return _context.Matiere.ToList();
        }

    }

}
