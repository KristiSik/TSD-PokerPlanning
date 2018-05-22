using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPokerBackend.Models.PostRequestBodyModels
{
    public class UserTokenAndTableToken
    {
        public string UserToken { get; set; }
        public string TableToken { get; set; }
    }
}
