using Microsoft.EntityFrameworkCore;
using ProjetBrima.Data;
using ProjetBrima.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjetBrima.Services
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _context;

        // Injection du contexte de la base de données
        public NoteService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Méthode pour ajouter une note dans la base de données
        public void AjouterNote(Note note)
        {
            var eleve = _context.Eleves.FirstOrDefault(e => e.Id == note.EleveId);  // Récupère l'élève en fonction de son ID
            if (eleve != null)
            {
                note.NomEleve = eleve.Nom;
                note.PrenomEleve = eleve.Prenom;
            }

            _context.Notes.Add(note);  // Ajoute la note à la base de données
            _context.SaveChanges();    // Sauvegarde les modifications
        }


        // Méthode pour obtenir toutes les notes
        public List<Note> GetAllNotes()
        {
            return _context.Notes
                .Include(n => n.Eleve) // Assure-toi que la propriété "Eleve" est bien une navigation vers un objet Eleve
                .Select(n => new Note
                {
                    Id = n.Id,
                    Classe = n.Classe,
                    Matiere = n.Matiere,
                    EleveId = n.EleveId,
                    NomEleve = n.Eleve.Nom,
                    PrenomEleve = n.Eleve.Prenom,
                    Valeur = n.Valeur
                })
                .ToList();
        }


        // Méthode pour mettre à jour une note existante
        public bool UpdateNote(int id, Note updatedNote)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == id);
            if (note == null) return false;

            note.Classe = updatedNote.Classe;
            note.Matiere = updatedNote.Matiere;
            note.Eleve = updatedNote.Eleve;
            note.Valeur = updatedNote.Valeur;

            _context.SaveChanges();  // Sauvegarde les modifications dans la base de données
            return true;
        }

        // Méthode pour supprimer une note
        public bool DeleteNote(int id)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == id);
            if (note == null) return false;

            _context.Notes.Remove(note);  // Supprime la note de la base de données
            _context.SaveChanges();      // Sauvegarde les modifications
            return true;
        }
    }
}
