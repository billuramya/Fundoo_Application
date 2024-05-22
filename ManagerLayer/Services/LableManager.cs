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
    public class LableManager : ILableManager
    {
        private readonly ILableRepo lableRepo;
        public LableManager(ILableRepo lableRepo)
        {
            this.lableRepo = lableRepo;
            
        }
        public bool AddLable(int userId, int noteId, string LableName)
        {
            return lableRepo.AddLable(userId, noteId, LableName);
        }
        public IEnumerable<LableEntity> GetLables(int userId)
        {
            return lableRepo.GetLables(userId);   
        }
        public LableEntity UpdateLableName(int userId, int noteId, string LableName)
        {
            return lableRepo.UpdateLableName(userId, noteId, LableName);
        }
        public bool DeleteLabes(int lableId, int userId)
        {
            return lableRepo.DeleteLabes(lableId,userId);
        }
        public object ParticularLable(int NoteId, string LableName)
        {
            return lableRepo.ParticularLable(NoteId, LableName);
        }
    }
}
