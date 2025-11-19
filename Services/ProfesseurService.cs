using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public class ProfesseurService : IProfesseurService
    {
        private readonly ApplicationDbContext _context;

        public ProfesseurService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Professeur>> GetAllProfesseursAsync()
        {
            return await _context.Professeurs.ToListAsync();
        }

        public async Task<Professeur> GetProfesseurByIdAsync(int id)
        {
            return await _context.Professeurs.FindAsync(id);
        }

        public async Task AddProfesseurAsync(Professeur Professeur)
        {
            _context.Professeurs.Add(Professeur);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProfesseurAsync(Professeur Professeur)
        {
            var existingProfesseur = await _context.Professeurs.FindAsync(Professeur.Id);
            if (existingProfesseur != null)
            {
                existingProfesseur.Prenom = Professeur.Prenom;
                existingProfesseur.Nom = Professeur.Nom;
                existingProfesseur.NumeroCIN = Professeur.NumeroCIN;
                existingProfesseur.EmailPersonnel = Professeur.EmailPersonnel;
                existingProfesseur.Telephone = Professeur.Telephone;
                existingProfesseur.Adresse = Professeur.Adresse;
                existingProfesseur.EmailInstitutionnel = Professeur.EmailInstitutionnel;
                existingProfesseur.Matiere = Professeur.Matiere;


                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteProfesseurAsync(int id)
        {
            var Professeur = await _context.Professeurs.FindAsync(id);
            if (Professeur != null)
            {
                _context.Professeurs.Remove(Professeur);
                await _context.SaveChangesAsync();
            }
        }
    }
}
