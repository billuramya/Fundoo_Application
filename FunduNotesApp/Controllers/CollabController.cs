using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FunduNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        private readonly ICollabManager collabManager;
        public CollabController(ICollabManager collabManager)
        {
            this.collabManager = collabManager;
        }
        [Authorize]
        [HttpPost("AddCollaborator")]
        public IActionResult AddCollab(int NoteId,string CollabEmail)
        {
            int userid = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var res = collabManager.CreateCollab(userid, NoteId, CollabEmail);
            if(res!=null)
            {
                return Ok(new {Success=true,Message="Creation SuccessFuly",Data=res});
            }
            else {
                return BadRequest(new { Success = false, Message = "Creation Failed" });
            }
        }
        [Authorize]
        [HttpPut("UpdateCollab")]
        public IActionResult Update(int noteId,string CollabEmail)
        {
            int userid = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var res= collabManager.UpdateCollab(userid,noteId,CollabEmail);
            if(res != null)
            {
                return Ok(new { Success = true, Message = "Updation SuccessFuly", Data = res });
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Updation Failed" });
            }
        }
        [Authorize]
        [HttpGet("GetAllCollabs")]
        public IActionResult GetDat()
        {
            var res = collabManager.GetAllCollabs();
            if (res!=null)
            {
                return Ok(new { Success = true, Message = "Get the data SuccessFuly", Data = res });
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Data Getting Failed" });
            }
        }
        [Authorize]
        [HttpDelete("DeleteCollabById")]
        public IActionResult Delete(int Collabid)
        {
            var res= collabManager.DeleteCollab(Collabid);
            if (res != null)
            {
                return Ok(new { Success = true, Message = "Deleted SuccessFuly", Data = res });
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Deletion Failed" });
            }
        }
        [HttpGet("GetAllCollabByNote")]
        public IActionResult GetCollabDataByNotes(int noteId)
        {
            var res = collabManager.GetAllCollabsBtNote(noteId);
            if (res != null)
            {
                return Ok(new { Success = true, Message = "Get the data SuccessFuly", Data = res });
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Data Getting Failed" });
            }
        }
        [HttpGet]
        [Route("CountNumberOfCollab")]
        public IActionResult countCollab(int userId)
        {
            var res = collabManager.CountNumberOfCollab(userId);
            if (res != null)
            {
                return Ok(new { success = true, message = "Count the number of Notes", Data = res });

            }
            else
            {
                return BadRequest("Count is not Exists");
            }
        }
    }
}
