using ProjetBrima.Models;

namespace ProjetBrima.Services
{
    public interface INoteService
    {
        void AjouterNote(Note note);
        List<Note> GetAllNotes();
        bool UpdateNote(int id, Note note);
        bool DeleteNote(int id);

    }
}
