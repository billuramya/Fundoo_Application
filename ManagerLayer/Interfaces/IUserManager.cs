using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        UserEntity UserRegester(UserRegisterModel req);
        public string UserLogin(UserLogin login);
         UserEntity FullName(FullName fullName);
         object GetAllDetalis();
        bool EmailCheck(string Mail);
        public ForgotPasswardModel ForgotPassword(string Email);
        public bool UpdateTable(int userId, UpdateModel update);
        public bool DeleteTabe(string FirstName);
        public bool ResetPassword(string Email, ResetPassword password);
        public bool PerfectUserLogin(string Email, string Password);
        public int CountNumberOfUsers();
        public object GetTheDetailsByName(string FirstName);
        public TokenModel LoginMethod(UserLogin login);
        public string UserIdExistOrNot(UserRegisterModel model, int userId);
    }
}
