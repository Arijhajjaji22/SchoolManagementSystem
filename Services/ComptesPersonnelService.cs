using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public class ComptesPersonnelService : IComptesPersonnelService
    {
        private readonly ApplicationDbContext _context;

        public ComptesPersonnelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ComptesPersonnel>> GetAllAsync()
        {
            return await _context.ComptesPersonnels.ToListAsync();
        }

        public async Task<ComptesPersonnel> GetByIdAsync(int id)
        {
            return await _context.ComptesPersonnels.FindAsync(id);
        }

        public async Task<ComptesPersonnel> AddAsync(ComptesPersonnel personnel)
        {
            _context.ComptesPersonnels.Add(personnel);
            await _context.SaveChangesAsync();
            return personnel;
        }

        public async Task<ComptesPersonnel> UpdateAsync(int id, ComptesPersonnel personnel)
        {
            var existing = await _context.ComptesPersonnels.FindAsync(id);
            if (existing == null) return null;

            existing.Prenom = personnel.Prenom;
            existing.Nom = personnel.Nom;
            existing.NumeroCIN = personnel.NumeroCIN;
            existing.EmailPersonnel = personnel.EmailPersonnel;
            existing.Telephone = personnel.Telephone;
            existing.Adresse = personnel.Adresse;
            existing.EmailInstitutionnel = personnel.EmailInstitutionnel;
            existing.MotDePasse = personnel.MotDePasse;
            existing.Role = personnel.Role;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var personnel = await _context.ComptesPersonnels.FindAsync(id);
            if (personnel == null) return false;

            _context.ComptesPersonnels.Remove(personnel);
            await _context.SaveChangesAsync();
            return true;
        }
    }
   
}
