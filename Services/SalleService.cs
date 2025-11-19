using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public class SalleService : ISalleService
    {
        private readonly ApplicationDbContext _context;

        public SalleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Salle>> GetAllSallesAsync()
        {
            return await _context.Salles.ToListAsync();
        }

        public async Task<Salle> GetSalleByIdAsync(int id)
        {
            return await _context.Salles.FindAsync(id);
        }

        public async Task<Salle> AddSalleAsync(Salle salle)
        {
            _context.Salles.Add(salle);
            await _context.SaveChangesAsync();
            return salle;
        }

        public async Task<Salle> UpdateSalleAsync(int id, Salle salle)
        {
            var existingSalle = await _context.Salles.FindAsync(id);
            if (existingSalle == null) return null;

            existingSalle.NumSalle = salle.NumSalle;
            existingSalle.TypeSalle = salle.TypeSalle;
            existingSalle.Capacite = salle.Capacite;
            existingSalle.Departement = salle.Departement;
            existingSalle.Disponibilite = salle.Disponibilite;

            await _context.SaveChangesAsync();
            return existingSalle;
        }

        public async Task<bool> DeleteSalleAsync(int id)
        {
            var salle = await _context.Salles.FindAsync(id);
            if (salle == null) return false;

            _context.Salles.Remove(salle);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
