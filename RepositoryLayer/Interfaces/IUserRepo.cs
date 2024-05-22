using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepo
    {
        public UserEntity UserRegester(UserRegisterModel req);
        public string UserLogin(UserLogin login);


        public UserEntity FullName(FullName fullName);
        public object GetAllDetalis();
        public bool EmailCheck(string Mail);
        //public string ForgotPassWord(string email);
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
