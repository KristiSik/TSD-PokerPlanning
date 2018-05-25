using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPokerBackend.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Value { get; set; }
    }
}
