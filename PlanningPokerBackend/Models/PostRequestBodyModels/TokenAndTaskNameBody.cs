using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPokerBackend.Models.PostRequestBodyModels
{
    public class TokenAndTaskNameBody
    {
        public string Token { get; set; }
        public string TaskName { get; set; }
    }
}
