using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningPokerBackend.Models
{
    public class DataSeeder
    {
        private readonly PlanningPokerDbContext _context;
        public DataSeeder(PlanningPokerDbContext context)
        {
            _context = context;
        }
        public void SeedData()
        {
            if (_context.Users.Count() != 0)
            {
                return;
            }
            List<User> users = new List<User>() {
                new User() { FirstName = "Dave", LastName = "Murray", Email = "davemurray@mail.com", Password = "encrypted_password" },
                new User() { FirstName = "Adrian", LastName = "Smith", Email = "adriansmith@mail.com", Password = "my_password" },
                new User() { FirstName = "Steve", LastName = "Harris", Email = "steveharris@mail.com", Password = "my_password" },
                new User() { FirstName = "Janick", LastName = "Gers", Email = "janickgers@mail.com", Password = "my_password" },
                new User() { FirstName = "Nick", LastName = "McBrain", Email = "nickmcbrain@mail.com", Password = "my_password" },
                new User() { FirstName = "Bruce", LastName = "Dickinson", Email = "brucedickinson@mail.com", Password = "my_password" }
            };
            PlayTable playTable = new PlayTable() { Admin = users.First(), Participants = users.Skip(1).ToList(), Token = "abcdef" };
            _context.Users.AddRange(users);
            _context.PlayTables.Add(playTable);
            _context.SaveChanges();
        }
    }
}
