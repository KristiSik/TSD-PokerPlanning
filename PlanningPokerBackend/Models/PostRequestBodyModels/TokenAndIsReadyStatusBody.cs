using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPokerBackend.Models.PostRequestBodyModels
{
    public class TokenAndIsReadyStatusBody
    {
        public string UserToken { get; set; }
        public bool? IsReady { get; set; }
    }
}