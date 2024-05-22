using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Migrations;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly FunduContext context;
        private readonly IConfiguration _config;
  
        public UserRepo(FunduContext context , IConfiguration _config)
        {
            this.context = context;
            this._config = _config;
        } 
        public UserEntity UserRegester(UserRegisterModel req)
        {
           
            UserEntity user = new UserEntity();
            user.FirstName = req.FirstName;
            user.LastName = req.LastName;
            user.Email = req.Email;
            //user.Password=req.Password;
            user.Password = EncodePasswordToBase64(req.Password);
            user.CreatedDate = DateTime.Now;
            user.UpdatedDate = DateTime.Now;

            context.Add(user);
            context.SaveChanges();

            return user;

        }
        
        public UserEntity FullName(FullName fullName)
        {
            var result = context.Users.FirstOrDefault(x => x.FirstName==fullName.FirstName); 
            if(result.FirstName.Equals(fullName.FirstName) && result.LastName.Equals(fullName.LastName)) { 
                return result;
            }
            else
            {
                return null;
            }
        }
        

        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public static string DecodePassword(string password)
        {
            try
            {

                return Encoding.UTF8.GetString(Convert.FromBase64String(password));
            }
            catch (FormatException ex)
            {
                
                Console.WriteLine("Error decoding password: " + ex.Message);
               
                return null;
            }
        }
        

        private string GenerateToken(int UserId, string userEmail)
        {
            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
             var claims = new[]
            {
                new Claim("Email",userEmail),
                new Claim("UserId", UserId.ToString())
            };
           
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMonths(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string UserLogin(UserLogin login)
        {
            
            var userFromDb = context.Users.FirstOrDefault(x => x.Email == login.Email);

            if (userFromDb != null && DecodePassword(userFromDb.Password) == login.Password)
            {
                var token = GenerateToken(userFromDb.UserId, userFromDb.Email);
                return token;
            }
            else
            {

                return null;
            }
        }


        public ForgotPasswardModel ForgotPassword(string Email)
        {
            UserEntity User = context.Users.ToList().Find(user => user.Email == Email);
            ForgotPasswardModel forgotPassword = new ForgotPasswardModel();
            forgotPassword.Email = User.Email;
            forgotPassword.UserId = User.UserId;
            forgotPassword.Token = GenerateToken(User.UserId, User.Email);
            return forgotPassword;
        }




        public bool UpdateTable(int userId,UpdateModel update)
        {
            var updateResult = context.Users.FirstOrDefault(x => x.UserId == userId);
            if (updateResult != null)
            {
                updateResult.UserId = update.UserId;
                updateResult.FirstName = update.FirstName;
                updateResult.LastName = update.LastName;
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;

            }
        }

        public bool DeleteTabe(string FirstName)
        {
            var result = context.Users.FirstOrDefault(user => user.FirstName == FirstName);
            if (result != null)
            {
                context.Users.Remove(result);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public object GetAllDetalis()
        {
            var result = context.Users.ToList();
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        } 

        public bool ResetPassword(string Email , ResetPassword password)
        {
            UserEntity result = context.Users.ToList().Find(x => x.Email  == Email);
            if (result != null)
            {
                result.Password = EncodePasswordToBase64(password.ConformPassword);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EmailCheck(string Mail)
        {
            var res = context.Users.FirstOrDefault(x => x.Email == Mail);

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            var s = Regex.IsMatch(Mail, pattern);
            //Regex Reges = Regex.IsMatch(res, emailRegex);
            if (s && res != null)
            {
                return PerfectUserLogin(res.Email, res.Password);

            }
            else
            {
                return false;
            }

        }

        public bool PerfectUserLogin (string Email, string Password)
        {
            var userPassword = context.Users.FirstOrDefault(x => x.Password == Password);

            if (userPassword != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        } 
          
    //) find count of users in user table

        public int CountNumberOfUsers()
        {
            var res = context.Users.Count();
            if (res > 0)
            {
                return res;
            }
            else
            {
                return 0;
            }
        }
        //) search a user by name and show their details. 
        public object GetTheDetailsByName(string FirstName)
        {
            var res = context.Users.Where(x => x.FirstName == FirstName);
            if(res != null)
            {
                return res;
            }
            else
            {
                return null;
            }
        }

        public TokenModel LoginMethod(UserLogin login)
        {
            var res= context.Users.FirstOrDefault(x =>x.Email == login.Email );
            if(res != null)
            {
                TokenModel token = new TokenModel();
                if(login.Password==DecodePassword(res.Password) && login.Email == res.Email)
                {
                    token.Email = res.Email;
                    token.FirstName=res.LastName;
                    token.LastName=res.FirstName;
                    token.UserId = res.UserId;
                    return token;


                }
                return null;
            }
            else
            {
                return null;
            }
        }


        //-> check if user exist, then update the user data.Else if user doesn’t exist insert a new data 

        public string UserIdExistOrNot(UserRegisterModel model, int userId)
        {
            var res = context.Users.FirstOrDefault(x => userId == x.UserId);
            if (res != null)
            {
                res.FirstName = "Munisekhar";
                res.LastName = "billu";
                context.SaveChanges();
                return "Updated";

            }
            else
            {
                UserEntity user = new UserEntity(); 
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.Password = model.Password;
                context.Add(user);
                context.SaveChanges();
                return "Inserted";

            }

        }

    }
}
