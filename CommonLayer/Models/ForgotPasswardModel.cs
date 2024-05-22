using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
     public class ForgotPasswardModel
    {
        public string Email {  get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
    }
}
