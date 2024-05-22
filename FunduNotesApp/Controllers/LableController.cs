using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;

namespace FunduNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LableController : ControllerBase
    {
        private readonly ILableManager lableManager;
        public LableController(ILableManager lableManager)
        {
            this.lableManager = lableManager;
        }

        [Authorize]
        [HttpPost("AddLable")]
        public IActionResult RegisterLable(int noteId,string LableName)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result= lableManager.AddLable(userId,noteId,LableName);
            if (result!= null)
            {
                return Ok(new {Success= true,Message="Lable Is Added",Data=result});
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Lable Is Not Added" });
            }
        }
        [Authorize]
        [HttpGet("GetAllLables")]
        public IActionResult GetData()
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var res=lableManager.GetLables(userId);
            if(res!=null)
            {
                return Ok(new { Success = true, Message = "Get All Lables",Data=res });
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Some Thing Went Wrong" });
            }
        }
        [Authorize]
        [HttpPut("UpdateLableByName")]
        public IActionResult Update(int LableId,string LableName)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var res= lableManager.UpdateLableName(userId,LableId,LableName);
            if(res!=null) 
            {
                return Ok(new { Success = true, Message = "Updated Name Successfuly", Data = res });
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Updation Failed" });
            }
        }

        [Authorize]
        [HttpDelete("DeleteLable")]
        public IActionResult Delete(int LableId)
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var res =lableManager.DeleteLabes(LableId,userId);
            if (res != null)
            {
                return Ok(new { Success = true, Message = "Deleted Successfuly", Data = res });
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Deletion Failed" });
            }
        }

        
        //[HttpGet("GetAllLables")]
        //public IActionResult ParticulaNote(int noteId,string LableName)
        //{
        //    var res = lableManager.ParticularLable(noteId,LableName);
        //    if (res != null)
        //    {
        //        return Ok(new { Success = true, Message = "Get Particular Lables", Data = res });
        //    }
        //    else
        //    {
        //        return BadRequest(new { Success = false, Message = "Some Thing Went Wrong" });
        //    }
        //}
    }
}
