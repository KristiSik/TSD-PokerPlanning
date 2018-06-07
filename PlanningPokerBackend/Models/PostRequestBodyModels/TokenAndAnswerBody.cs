using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPokerBackend.Models.PostRequestBodyModels
{
    public class TokenAndAnswerBody
    {
        public string Token { get; set; }
        public string Answer { get; set; }
    }
}
