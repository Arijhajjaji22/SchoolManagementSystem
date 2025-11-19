using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public class DirecteurService : IDirecteurService
    {
        private readonly ApplicationDbContext _context;

        public DirecteurService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Directeur>> GetAllDirecteursAsync()
        {
            return await _context.Directeurs.ToListAsync();
        }
        
        public async Task<Directeur> GetDirecteurByIdAsync(int id)
        {
            return await _context.Directeurs.FindAsync(id);
        }

        public async Task AddDirecteurAsync(Directeur directeur)
        {
            _context.Directeurs.Add(directeur);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDirecteurAsync(Directeur directeur)
        {
            var existingDirecteur = await _context.Directeurs.FindAsync(directeur.Id);
            if (existingDirecteur != null)
            {
                existingDirecteur.Prenom = directeur.Prenom;
                existingDirecteur.Nom = directeur.Nom;
                existingDirecteur.NumeroCIN = directeur.NumeroCIN;
                existingDirecteur.EmailPersonnel = directeur.EmailPersonnel;
                existingDirecteur.Telephone = directeur.Telephone;
                existingDirecteur.Adresse = directeur.Adresse;
                existingDirecteur.EmailInstitutionnel = directeur.EmailInstitutionnel;

                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteDirecteurAsync(int id)
        {
            var directeur = await _context.Directeurs.FindAsync(id);
            if (directeur != null)
            {
                _context.Directeurs.Remove(directeur);
                await _context.SaveChangesAsync();
            }
        }
    }

}
