using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface INotesManager
    {
        public string CreateNodes(NotesModel notes, int Userid);
        public object GetAllNotes();
        public bool UpdateNoteById(NotesModel entity, int id);
        public bool DeleteNoteById(int id);
        public object GetNotesById(int userId, int NotesId);
        public bool ChangeColor(int noteId, string color);
        public NotesEntity IsAchiveNotes(int userId, int NoteId);
        public NotesEntity IsPinnedNote(int userId, int NoteId);
        public NotesEntity IsTrashNote(int userId, int NoteId);
        public NotesEntity AddRemainder(int userId, int NoteId, DateTime remainder);

        public object GetNoteDetails(string title, string description);
        // public string UserIdExist(NotesEntity note, int userId);
        public int CountNumberOfNotes(int noteId);
        public object FindNoteAreCreated(DateTime createAt);


    }
}
