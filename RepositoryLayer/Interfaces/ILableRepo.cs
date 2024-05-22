using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ILableRepo
    {
        public bool AddLable(int userId, int noteId, string LableName);
        public IEnumerable<LableEntity> GetLables(int userId);
        public LableEntity UpdateLableName(int userId, int noteId, string LableName);
        public bool DeleteLabes(int lableId, int userId);
        public object ParticularLable(int NoteId, string LableName);
    }
}
