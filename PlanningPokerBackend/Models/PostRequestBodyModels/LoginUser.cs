using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPokerBackend.Models.PostRequestBodyModels
{
    public class LoginUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}