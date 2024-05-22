using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class CollabRepo : ICollabRepo
    {
        private readonly FunduContext context;
        public CollabRepo(FunduContext context)
        {
            this.context = context;
        }
        public string CreateCollab(int userId,int noteId,string collabEmail)
        {
            try
            {
                var res = context.UserNotes.FirstOrDefault(x => x.UserId == userId && x.NoteId == noteId);
                if(res != null)
                {
                    CollabEntity collab = new CollabEntity();
                    collab.UserId = userId;
                    collab.NoteId = noteId;
                    collab.CollabEmail = collabEmail;
                    context.Add(collab);
                    context.SaveChanges();
                    return "Adding data successfuly";
                }
                else {
                    return "Adding Data Failed";
                }
            }catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public bool UpdateCollab(int UserId,int NoteId,string collabEmail)
        {
            try
            {
                var res = context.Collaborator.FirstOrDefault(x => x.UserId == UserId && x.NoteId == NoteId);
                if(res != null)
                {
                    res.CollabEmail = collabEmail;
                    context.SaveChanges();  
                    return true;
                }
                else
                {
                    return false;
                }
            }catch(Exception ex)
            {
                return false;
            }
        }
        public object GetAllCollabs()
        {
            var res= context.Collaborator.ToList();
            if(res != null)
            {
                return res;
            }
            else
            {
                return null;
            }
        }
        public bool DeleteCollab(int CollabId) {

            var res = context.Collaborator.FirstOrDefault(x => x.CollabId == CollabId);
            try
            {
                if (res != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }catch { return false; }
        }






        // 2)-> find collaborator in notes and show details of collaboration

        public object GetAllCollabsBtNote(int noteId)
        {
            var res = context.Collaborator.Where(x => x.NoteId==noteId).ToList();
            if (res != null)
            {
                return res;
            }
            else
            {
                return null;
            }
        }
        //-> find count of collaborators of a particular user
        public int CountNumberOfCollab(int userId)
        {
            var res = context.Collaborator.Where(a => a.UserId == userId).Count();
            if (res > 0)
            {
                return res;
            }
            else
            {
                return 0;
            }
        }

    }
}
