using Microsoft.AspNetCore.Mvc;
using ProjetBrima.Models;
using ProjetBrima.Services;

namespace ProjetBrima.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        // Injection du service de gestion des notes
        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        // Récupérer toutes les notes
        [HttpGet]
        public IActionResult GetAllNotes()
        {
            var notes = _noteService.GetAllNotes();
            return Ok(notes);
        }

        // Ajouter une nouvelle note
        [HttpPost]
        public IActionResult AjouterNote([FromBody] Note note)
        {
            if (note == null
      || string.IsNullOrWhiteSpace(note.Classe)
      || string.IsNullOrWhiteSpace(note.Matiere)
      || note.EleveId <= 0)
            {
                return BadRequest("Champs manquants ou invalides.");
            }

            _noteService.AjouterNote(note);  // Enregistrer la note dans la base de données
            return Ok(note);
        }


        // Mettre à jour une note existante
        [HttpPut("{id}")]
        public IActionResult UpdateNote(int id, [FromBody] Note updatedNote)
        {
            var result = _noteService.UpdateNote(id, updatedNote);
            if (!result)
            {
                return NotFound("Note non trouvée.");
            }
            return Ok(updatedNote);
        }

        // Supprimer une note
        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            var result = _noteService.DeleteNote(id);
            if (!result)
            {
                return NotFound("Note non trouvée.");
            }
            return NoContent();  // Supprimée avec succès
        }
    }
}
