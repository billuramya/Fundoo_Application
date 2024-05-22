using CommonLayer.Models;
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
    public class UserManager : IUserManager
    {
        private readonly IUserRepo iuserRepo;
        public UserManager(IUserRepo iuserRepo)
        {
            this.iuserRepo = iuserRepo;
        }
        public UserEntity UserRegester(UserRegisterModel req)
        {
            return iuserRepo.UserRegester(req);
        }
        public string UserLogin(UserLogin login)
        {
            return iuserRepo.UserLogin(login);

        }
        public UserEntity FullName(FullName fullName)
        {
            return iuserRepo.FullName(fullName);
        }
        public object GetAllDetalis()
        {
            return iuserRepo.GetAllDetalis();
        }
        public bool EmailCheck(string Mail)
        {
            return iuserRepo.EmailCheck(Mail);
        }
        public ForgotPasswardModel ForgotPassword(string Email)
        {
            return iuserRepo.ForgotPassword( Email);
        }
        public bool UpdateTable(int userId, UpdateModel update)
        {
            return (iuserRepo.UpdateTable(userId, update));
        }
        public bool DeleteTabe(string FirstName)
        {
            return iuserRepo.DeleteTabe(FirstName);
        }
        public bool ResetPassword(string Email, ResetPassword password)
        {
            return iuserRepo.ResetPassword(Email, password);
        }
        public bool PerfectUserLogin(string Email, string Password)
        {
            return iuserRepo.PerfectUserLogin(Email, Password);
        }
        public int CountNumberOfUsers()
        {
            return iuserRepo.CountNumberOfUsers();
        }
        public object GetTheDetailsByName(string FirstName)
        {
            return iuserRepo.GetTheDetailsByName(FirstName);
        }
        public TokenModel LoginMethod(UserLogin login)
        {
            return iuserRepo.LoginMethod(login);
        }
        public string UserIdExistOrNot(UserRegisterModel model, int userId)
        {
            return iuserRepo.UserIdExistOrNot(model, userId);
        }
    }
}
