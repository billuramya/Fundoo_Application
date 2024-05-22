using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ICollabRepo
    {
        public string CreateCollab(int userId, int noteId, string collabEmail);
        public bool UpdateCollab(int UserId, int NoteId, string collabEmail);
        public object GetAllCollabs();
        public bool DeleteCollab(int CollabId);
        public object GetAllCollabsBtNote(int noteId);
        public int CountNumberOfCollab(int userId);
    }
}
