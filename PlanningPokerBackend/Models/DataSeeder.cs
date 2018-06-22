using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPokerBackend.Models
{
    public class DataSeeder
    {
        private readonly PlanningPokerDbContext _context;
        public static List<User> Users { get; private set; } = new List<User>() {
                new User() { FirstName = "Dave", LastName = "Murray", Email = "davemurray@mail.com", Password = "W6ph5Mm5Pz8GgiULbPgzG37mj9g=", Token = "dave_token" },
                new User() { FirstName = "Adrian", LastName = "Smith", Email = "adriansmith@mail.com", Password = "W6ph5Mm5Pz8GgiULbPgzG37mj9g=", Token = "andrian_token" },
                new User() { FirstName = "Steve", LastName = "Harris", Email = "steveharris@mail.com", Password = "W6ph5Mm5Pz8GgiULbPgzG37mj9g=", Token = "steve_token" },
                new User() { FirstName = "Janick", LastName = "Gers", Email = "janickgers@mail.com", Password = "W6ph5Mm5Pz8GgiULbPgzG37mj9g=", Token = "janick_token" },
                new User() { FirstName = "Nick", LastName = "McBrain", Email = "nickmcbrain@mail.com", Password = "W6ph5Mm5Pz8GgiULbPgzG37mj9g=", Token = "nick_token" },
                new User() { FirstName = "Bruce", LastName = "Dickinson", Email = "brucedickinson@mail.com", Password = "W6ph5Mm5Pz8GgiULbPgzG37mj9g=", Token = "bruce_token" }
            };
        public DataSeeder(PlanningPokerDbContext context)
        {
            _context = context;
            // User.Password is encrypted from "password" 
            
        }
        public void SeedData()
        {
            if (_context.Users.Count() != 0)
            {
                return;
            }
            var users = Users;
            users.First().PlayTable = new PlayTable() { Admin = users.First(), Participants = users.Skip(1).ToList(), Token = "abcdef" };
            _context.Users.AddRange(users);
            _context.SaveChanges();
        }
    }
}
