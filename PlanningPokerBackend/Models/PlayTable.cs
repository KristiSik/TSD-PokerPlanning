using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPokerBackend.Models
{
    public class PlayTable
    {
        public int Id { get; set; }
        public User Admin { get; set; }
        public ICollection<User> Participants { get; set; }
    }
}