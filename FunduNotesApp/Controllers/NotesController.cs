using CommonLayer.Models;
using GreenPipes.Caching;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace FunduNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesManager notesManager;
        private readonly FunduContext context;
        private readonly IDistributedCache distributedCache;
        public NotesController(INotesManager notesManager, FunduContext context, IDistributedCache distributedCache)
        {
            this.notesManager = notesManager;
            this.context = context;
            this.distributedCache = distributedCache;
        }

        
        [HttpPost("CreateNotes")]
        public IActionResult Create(NotesModel notes)
        {
            //int id = (int)HttpContext.Session.GetInt32("userId");
            //int id = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var id = User.FindFirstValue("UserId");
            var result = notesManager.CreateNodes(notes,int.Parse(id));
            if (result != null)
            {
                return Ok(new  { Success = true, Message = "Notes Are Created", Data = result });
            }
            else
            {
                return BadRequest(new  { Success = false, Message = "Notes Are Not Created" });
            }
        }

        [HttpGet("GetAllNotes")]
        public IActionResult GetNodes()
        {
            var result = notesManager.GetAllNotes();
            if (result != null)
            {
                return Ok(new  { Success = true, Message = "Get All Notes Successfully", Data = result });
            }
            else
            {
                return BadRequest(new  { Success = false, Message = "Notes Are not getting" });
            }
        }


        [HttpPut("UpdateNotesById")]
        public IActionResult UpdateNotes(NotesModel notes,int noteid) {

            var res = notesManager.UpdateNoteById(notes, noteid); 
            if(res != null)
            {
                return Ok(new  { Success = true, Message = "Update SuccessFuly", Data = res });
            }
            else
            {
                return BadRequest(new  { Success = false, Message = "Update SuccessFuly", Data = res });
            }
        
        }

        [HttpDelete("DeleteNotesById")]
        public IActionResult DeleteNotes(int id)
        {
            var result=notesManager.DeleteNoteById(id);
            if(result != null)
            {
                return Ok(new Response<NotesEntity> { Success = true, Message = "Deleted Notes Successfully" ,Data=result});
            }
            else
            {
                return BadRequest(new Response<NotesEntity> { Success = false, Message = "Deletion Not Successfull",Data=result });
            }
        }

        [HttpGet("GetNotesById")] 

        public IActionResult GetNotes(int userId,int NoteId)
        {
            var result= notesManager.GetNotesById(userId, NoteId);
            if(result != null)
            {
                return Ok(new Response<NotesEntity> { Success = true,Message="Successfully get the Notes",Data= (NotesEntity)result });
            }
            else
            {
                return BadRequest(new Response<NotesEntity> { Success = false, Message = "Not Getting Notes", Data = (NotesEntity)result });
            }
        }
        [HttpPut("ChangeColor")]
        public IActionResult ChangeNoteColor(int noteId,string color)
        {
            var result =notesManager.ChangeColor(noteId,color);
            if(result != null)
            {
                return Ok(new  { Success = true,Message="Color Change Successfully",Data=result});
            }
            else
            {
                return BadRequest(new  { Success = false, Message = "Color Not Chage" });
            }
        }
        [Authorize]

        [HttpPut("IsAchive")] 
        public IActionResult AchiveNotes(int noteid) {
            //var UserId = User.FindFirstValue("UserId");
            //int.Parse(UserId)
            int UserId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = notesManager.IsAchiveNotes(UserId, noteid);
            if (result != null)
            {
                return Ok(new Response<NotesEntity> { Success = true, Message = "Note is Achive", Data = result });
            }
            else
            {
                return BadRequest(new Response<NotesEntity> { Success = false, Message = "Note Is Not Achive" });
            }
        }
        [Authorize]
        [HttpPost("IsPinned")]
        public IActionResult PinnedNotes(int NoteId)
        {
            int UserId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result =notesManager.IsPinnedNote(UserId, NoteId);
            if(result != null) { 
                return Ok(new Response<NotesEntity> { Success = true, Message = "Note is Pinned", Data = result });
        }
            else
            {
                return BadRequest(new Response<NotesEntity> { Success = false, Message = "Note Is Not Pinned" });
            }
        }
        [Authorize]
        [HttpPut("IsTrash")]
        public IActionResult IsTrashNotes(int noteid)
        {
            try
            {
                int UserId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
                var result = notesManager.IsTrashNote(UserId, noteid);
                if (result != null)
                {
                    return Ok(new Response<NotesEntity> { Success = true, Message = "Note Trashed Successfuly", Data = result });
                }
                else
                {
                    return BadRequest(new Response<NotesEntity> { Success = false, Message = "Note Is Not Trashed" });
                }
            }catch (Exception ex) { return BadRequest(ex.Message); }
        }
        [Authorize]
        [HttpPut("AddRemainder")]
        public IActionResult NoteRemainder(int NotesId,DateTime remainder)
        {
            int userid = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var res=notesManager.AddRemainder(userid, NotesId, remainder);
            if(res != null)
            {
                return Ok(new Response<NotesEntity> { Success = true, Message = "Remainder Added Successfuly", Data = res });
            }
            else
            {
                return BadRequest(new Response<NotesEntity> { Success = false, Message = "Remainder Failed" });
            }
        }
        [HttpGet]
        [Route("Get/{Title}/{IsAchive}")]
        public async Task<List<NotesEntity>> GetAll(string Title,bool IsAchive)
        {
            if(!IsAchive)
            {
                  return context.UserNotes.Where(x => x.Title == Title).OrderByDescending(x => x.CreatedAt).ToList(); 
            }
            string CachKey = Title;
            byte[] CachedData = await distributedCache.GetAsync(CachKey);
            List<NotesEntity> notesEntities = new();
            if(CachedData != null)
            {
                var CatchedDataString=Encoding.UTF8.GetString(CachedData);
                notesEntities=JsonSerializer.Deserialize<List<NotesEntity>>(CatchedDataString);

            }
            else
            {
                notesEntities = context.UserNotes.Where(x => x.Title == Title).OrderByDescending(x => x.CreatedAt).ToList();
                string cachedDataString = JsonSerializer.Serialize(notesEntities);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5)) 
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                await distributedCache.SetAsync(CachKey, dataToCache, options);
            }
            
            return notesEntities;
        }

        [HttpGet("GetTheNotesByTitleAndDis")]
        public IActionResult GetDetailsByTitleAndDis(string Title, string discription)
        {
            var res = notesManager.GetNoteDetails(Title, discription);
            if (res != null)
            {
                return Ok(new Response<NotesEntity> { Success = true, Message = "Get the Notes Successfully", Data = (NotesEntity)res });
            }
            else
            {
                return BadRequest(new Response<NotesEntity> { Success = false, Message = "Get the Notes Failed" });
            }
        }

        [HttpGet]
        [Route("CountNumberOfNotes")]
        public IActionResult countUsers(int noteId)
        {
            var res = notesManager.CountNumberOfNotes(noteId);
            if (res != null)
            {
                return Ok(new { success = true, message = "Count the number of Notes", Data = res });

            }
            else
            {
                return BadRequest(new { success = false, message = "Count the number of Notes not Exist", Data = res });
            }
        }

        [HttpGet("NoteCreatedDate")]
        public IActionResult CreateAtNote(DateTime createAt)
        {
            var res = notesManager.FindNoteAreCreated(createAt);
            if (res != null)
            {
                return Ok(new { success = true, message = "Find the note Created Date", Data = res });

            }
            else
            {
                return BadRequest(new { success = false, message = "its Not Find", Data = res });
            }
        }


    }
}

