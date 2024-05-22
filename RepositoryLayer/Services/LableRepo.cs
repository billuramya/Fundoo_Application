using Microsoft.EntityFrameworkCore;
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
    public class LableRepo : ILableRepo
    {
        private readonly FunduContext context;
        public LableRepo(FunduContext context)
        {
            this.context = context;
        }

        public bool AddLable(int userId,int noteId,string LableName)
        {
            try
            {
                var res = context.UserNotes.FirstOrDefault(x => x.UserId == userId && x.NoteId == noteId);
                if (res != null)
                {
                    LableEntity lable = new LableEntity();
                    lable.UserId = userId;
                    lable.NoteId = noteId;
                    lable.LableName = LableName;
                    context.Add(lable);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            
            }   
        }
        public IEnumerable<LableEntity> GetLables(int userId)
        {
            try
            {
                var res = context.Lable.Where(x => x.UserId == userId).ToList();
                if (res != null)
                {
                    return res;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex) { return null; }
        }
        public LableEntity UpdateLableName(int userId, int NoteId,string LableName)
        {
            try
            {
                var res = context.Lable.FirstOrDefault(x => x.UserId == userId && x.NoteId == NoteId);
                if(res != null)
                {
                    res.LableName = LableName;
                    context.Entry(res).State = EntityState.Modified;
                    return res;
                }
                else
                {
                    return null;
                }

            }catch(Exception ex)
            {
                return null;
            }
        }
        public bool DeleteLabes(int lableId,int userId) {
            try
            {
                var res = context.Lable.FirstOrDefault(x => x.LableId == lableId && x.UserId==userId);
                if( res != null)
                {
                    context.Remove(res);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }catch (Exception ex) { return false; }

        } 







        //-> find notes that belongs to a particular label name 
        public object ParticularLable(int NoteId,string LableName)
        {
            var res = context.Lable.FirstOrDefault(x => x.NoteId==NoteId &&  x.LableName==LableName);
            if(res != null)
            {
                return res;
            }
            else
            {
                return null;
            }
        }
    }
}
