using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPokerBackend.Models.PostRequestBodyModels
{
    public class TokenAndEmailBody
    {
        public string UserToken { get; set; }
        public string ParticipantEmail { get; set; }
    }
}
