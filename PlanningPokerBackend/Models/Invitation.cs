using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPokerBackend.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public User Inviter { get; set; }
        public User Participant { get; set; }
        public PlayTable PlayTable { get; set; }
        public string Token { get; set; }
    }
}