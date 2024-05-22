using CommonLayer.Models;
using ManagerLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;


namespace FunduNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly IBus bus;
        public UserController(IUserManager userManager, IBus bus)
        {
            this.userManager = userManager;
            this.bus = bus;
        }
        [HttpPost]
        [Route("UserRegister")]
        public IActionResult Reg(UserRegisterModel model)
        {
            var LoginRes = userManager.EmailCheck(model.Email);

            if (LoginRes)
            {
                return BadRequest("Already Exists");
            }
            else
            {

                var res = userManager.UserRegester(model);
                if (res != null)
                {
                    return Ok(new { Success = true, Message = "Register Success!", Data = res });
                }
                else
                {
                    return BadRequest(new { Success = true, Message = "hello", Data = res });
                }
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult UserLogin(UserLogin login)
        {
            var res = userManager.UserLogin(login);
            if (res != null)
            {
                return Ok(new { Success = true, Message = "login  Success!", Data = res });
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Login Failed", Data = res });
            }
        }
        [HttpPost]
        [Route("FullName")]
        public IActionResult UserFullName(FullName fullName)
        {
            var res = userManager.FullName(fullName);
            if (res != null)
            {
                return Ok(new { Success = true, Message = "login  Success!", Data = res });
            }
            else
            {
                return BadRequest(new { Success = true, Message = "hello", Data = res });
            }
        }
        [HttpGet]
        [Route("GetAllData")]
        public IActionResult GetAll()
        {
            var res = userManager.GetAllDetalis();
            if (res != null)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest("not found,,!");
            }

        }
        [HttpGet]
        [Route("CheckEmail")]
        public IActionResult Get(string mail)
        {
            var res = userManager.EmailCheck(mail);
            if (res != null)
            {
                return Ok(new { Success = true, Message = "Email Already Exist" });
            }
            else
            {
                return BadRequest(new { Success = true, Message = "Create New Email" });
            }

        }
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string Email)
        {

            var password = userManager.ForgotPassword(Email);

            if (password != null)
            {
                Send send = new Send();
                ForgotPasswardModel forgotPasswordModel = userManager.ForgotPassword(Email);
                send.SendMail(forgotPasswordModel.Email, forgotPasswordModel.Token);
                Uri uri = new Uri("rabbitmq:://localhost/FunDooNotesEmailQueue");
                var endPoint = await bus.GetSendEndpoint(uri);
                await endPoint.Send(forgotPasswordModel);
                return Ok(new { Success = true, Message = "Mail sent Successfully", Data = password.Token });
            }
            else
            {
                return BadRequest(new { Success = false, Message = "Email Does not Exist" });
            }


        }
        [HttpPut]
        [Route("UpdateByUserId")]
        public IActionResult UpdateUserDetails(int userId, UpdateModel model)
        {
            var res = userManager.UpdateTable(userId, model);
            if (res != null)
            {
                return Ok(new { Success = true, Message = "Details Updated Successfuly", Data = res });

            }
            else
            {
                return BadRequest(new { Success = false, Message = "Details are not Updated" });
            }
        }
        [HttpDelete]
        [Route("DeleteByFirstName")]

        public IActionResult DeleteUser(string FirstName)
        {
            var res = userManager.DeleteTabe(FirstName);
            if (res != null)
            {
                return Ok(new { Success = true, Message = "Details are Deleted", Data = res });

            }
            else
            {
                return BadRequest(new { Success = false, Message = "Details are Not Deleted" });
            }
        }


        [Authorize]
        [HttpPut("ResetPassword")]
        public IActionResult ResetPassWord(ResetPassword reset)
        {
            string Email = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;

            var res = userManager.ResetPassword(Email, reset);
            if (res)
            {
                return Ok(new { success = true, message = "Password Reset is done", data = res });

            }
            else
            {
                return BadRequest("Password is not Updated");
            }

        }
        [HttpPost("PerfectLoginUser")]
        public IActionResult PerfecUser(string Email, string Password)
        {
            bool Content = userManager.PerfectUserLogin(Email, Password);
            if (Content != null)
            {
                return Ok(new { success = true, message = "Pefect Login" });

            }
            else
            {
                return BadRequest("Enter Correct Formate");
            }
        }
        [HttpGet]
        [Route("CountNumberOfUsers")]
        public IActionResult countUsers()
        {
            var res = userManager.CountNumberOfUsers();
            if (res != null)
            {
                return Ok(new { success = true, message = "Count the number of Users", Data = res });

            }
            else
            {
                return BadRequest("Count is not Exists");
            }
        }
        [HttpGet]
        [Route("GetAllUsersByName")]
        public IActionResult GetUsersByName(string FirstName)
        {
            var res = userManager.GetTheDetailsByName(FirstName);
            if (res != null)
            {
                return Ok(new { success = true, Message = "Get All Users", Data = res });
            }
            else
            {
                return BadRequest(new { success = true, Message = "Get Users Are not Exist", Data = res });
            }

        }

        [HttpPost("LoginMethod")]
        public IActionResult LoginMethod(UserLogin model)
        {
            var login = userManager.LoginMethod(model);
            if (login != null)
            {

                int userId = Convert.ToInt32(login.UserId);
                HttpContext.Session.SetInt32("userID", userId);
                return Ok(new { IsSuccess = true, Message = "User Login Successful", Data = login });
            }
            else
            {
                return NotFound("User Login Unsuccessful");
            }

        }

        [HttpPut("InserAndUpdate")]
        public IActionResult InsderAndCreate(UserRegisterModel model, int userId)
        {


            var res = userManager.UserIdExistOrNot(model, userId);
            if (res != null)
            {
                return Ok(new { success = true, message = "Updation is done", data = res });

            }
            else
            {
                return BadRequest(new { success = false, message = "Updation is Falied", data = res });
            }

        }
    }
}
