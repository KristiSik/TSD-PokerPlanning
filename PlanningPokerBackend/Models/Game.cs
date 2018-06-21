using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPokerBackend.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public PlayTable PlayTable { get; set; }
        public List<Answer> Answers { get; set; }
        public bool IsFinished { get; set; } = false;
        public Game()
        {
            Answers = new List<Answer>();
        }
    }
}
