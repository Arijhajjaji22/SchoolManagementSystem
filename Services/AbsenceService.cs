using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public class AbsenceService : IAbsenceService
    {
        private readonly ApplicationDbContext _context;

        public AbsenceService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Enregistrer l'absence ou la présence d'un élève
        public async Task EnregistrerAbsenceAsync(int eleveId, int matiereId, string status)
        {
            var absence = new Absence
            {
                EleveId = eleveId,
                MatiereId = matiereId,
                Status = status
            };

            _context.Absence.Add(absence);
            await _context.SaveChangesAsync();
        }

        // Obtenir les absences d'un élève donné
        public async Task<List<Absence>> GetAbsencesByEleveAsync(int eleveId)
        {
            return await _context.Absence
                .Where(a => a.EleveId == eleveId)
                .ToListAsync();
        }
        public async Task<Absence?> GetDernierStatutAsync(int eleveId)
        {
            return await _context.Absence
                .Where(a => a.EleveId == eleveId)
            
                .FirstOrDefaultAsync(); // ou filtrer par MatiereId si nécessaire
        }

    }

}
