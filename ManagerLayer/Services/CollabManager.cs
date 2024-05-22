using ManagerLayer.Interfaces;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Services
{
   public class CollabManager: ICollabManager
    {
        private readonly ICollabRepo collabRepo;
        public CollabManager(ICollabRepo collabRepo)
        {
            this.collabRepo = collabRepo;
        }
        public string CreateCollab(int userId, int noteId, string collabEmail)
        {
            return collabRepo.CreateCollab(userId, noteId, collabEmail);
        }
        public bool UpdateCollab(int UserId, int NoteId, string collabEmail)
        {
            return collabRepo.UpdateCollab(UserId, NoteId, collabEmail);
        }
        public object GetAllCollabs()
        {
            return collabRepo.GetAllCollabs();
        }
        public bool DeleteCollab(int CollabId)
        {
            return collabRepo.DeleteCollab(CollabId);
        }
        public object GetAllCollabsBtNote(int noteId)
        {
            return collabRepo.GetAllCollabsBtNote(noteId);
        }
        public int CountNumberOfCollab(int userId)
        {
            return collabRepo.CountNumberOfCollab(userId);
        }
    }
}
