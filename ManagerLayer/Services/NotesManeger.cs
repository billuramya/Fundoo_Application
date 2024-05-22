using CommonLayer.Models;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Services
{
    public class NotesManeger : INotesManager
    {
        private readonly INotesRepo notesRepo;
        public NotesManeger(INotesRepo notesRepo)
        {
            this.notesRepo = notesRepo;
        }
        public string CreateNodes(NotesModel notes, int Userid)
        {
            return notesRepo.CreateNodes(notes, Userid);
        }
        public object GetAllNotes()
        {
            return notesRepo.GetAllNotes();
        }
        public bool UpdateNoteById(NotesModel entity, int id)
        {
            return notesRepo.UpdateNoteById(entity, id);
        }
        public bool DeleteNoteById(int id)
        {
            return notesRepo.DeleteNoteById(id);
        }
        public object GetNotesById(int userId, int NotesId)
        {
            return notesRepo.GetNotesById(userId, NotesId);
        }
        public bool ChangeColor(int noteId, string color)
        {
            return notesRepo.ChangeColor(noteId, color);

        }
        public NotesEntity IsAchiveNotes(int userId, int NoteId)
        {
            return notesRepo.IsAchiveNotes(userId, NoteId);
        }
        public NotesEntity IsPinnedNote(int userId, int NoteId)
        {
            return notesRepo.IsPinnedNote(userId, NoteId);
        }
        public NotesEntity IsTrashNote(int userId, int NoteId)
        {
            return notesRepo.IsTrashNote(userId, NoteId);
        }
        public NotesEntity AddRemainder(int userId, int NoteId, DateTime remainder)
        {
            return notesRepo.AddRemainder(userId, NoteId, remainder);
        }
        public object GetNoteDetails(string title, string description)
        {
            return notesRepo.GetNoteDetails(title, description);
        }

        public int CountNumberOfNotes(int noteId)
        {
            return notesRepo.CountNumberOfNotes(noteId);
        }
        public object FindNoteAreCreated(DateTime createAt)
        {
            return notesRepo.FindNoteAreCreated(createAt);
        }
    }
}
