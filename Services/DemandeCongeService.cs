using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public class DemandeCongeService : IDemandeCongeService
    {
        private readonly ApplicationDbContext _context;

        public DemandeCongeService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Récupérer toutes les demandes de congé
        public async Task<List<DemandeConge>> GetDemandesAsync()
        {
            return await _context.DemandesConge.ToListAsync();
        }

        // Récupérer une demande de congé par ID
        public async Task<DemandeConge> GetDemandeByIdAsync(int id)
        {
            return await _context.DemandesConge.FirstOrDefaultAsync(d => d.Id == id);
        }

        // Ajouter une nouvelle demande de congé
        public async Task<bool> AddDemandeAsync(DemandeConge demande)
        {
            _context.DemandesConge.Add(demande);
            return await _context.SaveChangesAsync() > 0;
        }

        // Mettre à jour le statut de la demande (ex: Approuvé, Refusé)
        public async Task<bool> UpdateDemandeStatutAsync(int id, string statut)
        {
            var demande = await _context.DemandesConge.FirstOrDefaultAsync(d => d.Id == id);
            if (demande == null) return false;

            demande.Statut = statut;
            return await _context.SaveChangesAsync() > 0;
        }
        // Modifier une demande
        public async Task<bool> UpdateDemandeAsync(DemandeConge demande)
        {
            _context.DemandesConge.Update(demande);
            return await _context.SaveChangesAsync() > 0;
        }

        // Supprimer une demande
        public async Task<bool> DeleteDemandeAsync(int id)
        {
            var demande = await _context.DemandesConge.FirstOrDefaultAsync(d => d.Id == id);
            if (demande == null) return false;

            _context.DemandesConge.Remove(demande);
            return await _context.SaveChangesAsync() > 0;
        }

    }

}
