using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public class EleveService : IEleveService
    {
        private readonly ApplicationDbContext _context;

        public EleveService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Eleve> GetEleveByIdAsync(int id)
        {
            return await _context.Eleves.FindAsync(id);
        }

        public async Task<IEnumerable<Eleve>> GetAllElevesAsync()
        {
            return await _context.Eleves.ToListAsync();
        }
        public async Task CreateEleveAsync(Eleve eleve, IFormFile bulletinFile)
        {
            var existingEleve = await _context.Eleves
                                              .FirstOrDefaultAsync(e => e.Email == eleve.Email);

            if (existingEleve != null)
                throw new InvalidOperationException("Cet élève est déjà inscrit avec cet email.");

            if (bulletinFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await bulletinFile.CopyToAsync(memoryStream);
                    eleve.BulletinFile = memoryStream.ToArray();
                }
            }

            _context.Eleves.Add(eleve);
            await _context.SaveChangesAsync();

  

         
            await _context.SaveChangesAsync(); // Sauvegarder le paiement en attente
        }


        public async Task UpdateEleveAsync(Eleve eleve, IFormFile bulletinFile)
        {
            if (bulletinFile != null)
            {
                // Si un fichier précédent existe, on peut choisir de le supprimer (optionnel selon votre logique)
                if (eleve.BulletinFile != null)
                {
                    // Ici vous pouvez ajouter de la logique pour gérer la suppression du fichier si nécessaire
                }

                using (var memoryStream = new MemoryStream())
                {
                    await bulletinFile.CopyToAsync(memoryStream);
                    eleve.BulletinFile = memoryStream.ToArray(); // Remplace le fichier par le nouveau
                }
            }

            _context.Eleves.Update(eleve);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteEleveAsync(int id)
        {
            var eleve = await _context.Eleves.FindAsync(id);
            if (eleve != null)
            {
                _context.Eleves.Remove(eleve);
                await _context.SaveChangesAsync();
            }
        }
        // Vérifie si l'élève a effectué un paiement
        public async Task<bool> EstPayéParStripeAsync(int eleveId)
        {
            var paiement = await _context.Paiements
                                         .Where(p => p.EleveId == eleveId && p.EstPayé)
                                         .FirstOrDefaultAsync();

            return paiement != null;
        }

        // Récupère les élèves qui ont effectué un paiement
        public async Task<IEnumerable<Eleve>> GetElevesAyantPayéAsync()
        {
            // On va chercher tous les paiements "EstPayé == true", puis on récupère les élèves associés.
            var eleves = await _context.Paiements
                .Where(p => p.EstPayé)
                .Include(p => p.Eleve) // On inclut l'élève lié au paiement
                .Select(p => p.Eleve)
                .Distinct() // Évite les doublons si un élève a payé plusieurs fois
                .ToListAsync();

            return eleves;
        }

        public async Task UpdatePaiementStatus(int eleveId, bool estPayé)
        {
            var paiement = await _context.Paiements
                .FirstOrDefaultAsync(p => p.EleveId == eleveId && !p.EstPayé);

            if (paiement != null)
            {
                paiement.EstPayé = estPayé;
                _context.Paiements.Update(paiement);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Eleve>> GetElevesParClasseAsync(string classe)
        {
            return await _context.Eleves
                .Where(e => e.Classe == classe)
                .ToListAsync();
        }

    }
}
