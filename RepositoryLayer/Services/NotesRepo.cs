using CommonLayer.Models;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class NotesRepo : INotesRepo
    {
        private readonly FunduContext Context;
        public NotesRepo(FunduContext Context)
        {
            this.Context = Context;
        }
        public string CreateNodes(NotesModel notes, int Userid)
        {
            try
            {
                if (Userid != 0)
                {
                    UserEntity user = Context.Users.FirstOrDefault(x => x.UserId == Userid);
                    if (user != null)
                    {
                        NotesEntity entity = new NotesEntity();
                        entity.UserId = Userid;
                        entity.Title = notes.Title;
                        entity.Description = notes.Description;
                        entity.Color = notes.Color;
                        entity.Remainder = notes.Remainder;
                        entity.IsArchive = notes.IsArchive;
                        entity.IsPinned = notes.IsPinned;
                        entity.IsTrash = notes.IsTrash;
                        entity.CreatedAt = DateTime.Now;
                        entity.ModifiedAt = DateTime.Now;
                        Context.Add(entity);
                        Context.SaveChanges();
                        return "Created Notes Successfully";
                    }
                    
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        } 

        public object GetAllNotes()
        {
            try
            {
                var res = Context.UserNotes.ToList();
                if (res != null)
                {
                    return res;
                }
                else
                {
                    return null;
                }
            }catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool UpdateNoteById(NotesModel entity, int id)
        {
            try
            {
                var res = Context.UserNotes.FirstOrDefault(x => x.NoteId == id);
                if(res != null)
                {
                    res.Title = entity.Title;
                    res.Description = entity.Description;
                    res.Color = entity.Color;
                    res.Remainder = entity.Remainder;
                    res.IsArchive = entity.IsArchive;
                    res.IsPinned = entity.IsPinned;
                    res.IsTrash = entity.IsTrash;
                    res.CreatedAt = DateTime.Now;
                    
                    res.NoteId = id; 
                  
                    Context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }catch(Exception ex) {
                return false;
            }
        }  

        public bool DeleteNoteById(int id) {

            try
            {
                var res = Context.UserNotes.FirstOrDefault(x => x.NoteId == id);
                if(res != null)
                {
                    Context.Remove(res);
                    Context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }catch (Exception ex) { return false; }
        }  

        public object GetNotesById(int userId,int NotesId)
        {
            try
            {
                var res = Context.UserNotes.FirstOrDefault(x => x.UserId == userId && x.NoteId == NotesId);
                if(res! != null)
                {
                    return res;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }  
        public bool ChangeColor(int noteId,string color)
        {
            try
            {
                var res = Context.UserNotes.FirstOrDefault(x => x.NoteId == noteId);
                if (res != null)
                {
                    res.Color = color;
                    Context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }catch(Exception ex) { return false; }
        } 
        public NotesEntity IsAchiveNotes(int userId ,int NoteId) {

            try
            {
                var res = Context.UserNotes.FirstOrDefault(x => x.UserId == userId && x.NoteId == NoteId);
                if (res != null)
                {
                    res.IsArchive = !res.IsArchive;
                    Context.SaveChanges();
                    return res;
                }
                else
                {
                    return null;
                }
            }catch (Exception ex) { return null; }
        }
        public NotesEntity IsPinnedNote(int userId ,int NoteId)
        {
            try { 
            var res = Context.UserNotes.FirstOrDefault(x => x.UserId == userId && x.NoteId == NoteId);
                if (res.IsPinned == true)
                {
                    res.IsPinned = false;
                    Context.Entry(res).State=EntityState.Modified;
                    Context.SaveChanges();
                    return res;
                }
                else
                {
                    res.IsPinned = true;
                    Context.Entry(res).State =EntityState.Modified;
                    Context.SaveChanges();
                    return res;
                }
            }catch (Exception ex) { return null; }
        }  

        public NotesEntity IsTrashNote(int userId ,int NoteId)
        {
            try
            {
                var res = Context.UserNotes.FirstOrDefault(x => x.UserId == userId && x.NoteId == NoteId);
                if (res != null)
                {
                    res.IsTrash = !res.IsTrash;
                    Context.SaveChanges();
                    return res;
                }
                else
                {
                    return null;
                }
            }catch(Exception ex) { return null; }
        }

        public NotesEntity AddRemainder(int userId, int NoteId, DateTime remainder)
        {
            try
            {
                var res = Context.UserNotes.Where(x => x.UserId == userId && x.NoteId == NoteId).FirstOrDefault();
                if (res == null)
                {
                    return null;
                }
                else
                {
                    if (res.Remainder > DateTime.Now)
                    {
                        res.Remainder = remainder;
                        Context.Entry(res).State = EntityState.Modified;
                        Context.SaveChanges();
                        return res;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex) { return null; }
        }
        
        
        
        //1)-> search note using title and description, show details of that note
       
        public object GetNoteDetails(string title,string description)
        {
            var result=Context.UserNotes.FirstOrDefault(x => x.Title==title && x.Description==description);
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        
        //-> count no of notes belongs to a particular user 
        public int CountNumberOfNotes(int noteId)
        {
            var res = Context.UserNotes.Where(a => a.NoteId==noteId).Count();
            if (res > 0)
            {
                return res;
            }
            else
            {
                return 0;
            }
        }
    
    //-> find note on the basis of the date the notes were created
    
        public object FindNoteAreCreated(DateTime createAt)
        {
            var res= Context.UserNotes.Where(x => x.CreatedAt<=createAt).FirstOrDefault();
            if (res != null)
            {
                return res;
            }
            else
            {
                return "Note is Not Created";
            }
        } 
    
    }
}
