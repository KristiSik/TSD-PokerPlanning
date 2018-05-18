using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlanningPokerBackend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanningPokerBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UsersController : Controller
    {
        private readonly PlanningPokerDbContext _context;

        public UsersController(PlanningPokerDbContext context)
        {
            _context = context;
            if (_context.Users.Count() == 0)
            {
                _context.Users.AddRange(
                    new User() { Email = "mail@mail.com", FirstName = "Kristian", LastName = "Sik", IsOnline = true },
                    new User() { Email = "mail@mail.com", FirstName = "Dmytro", LastName = "Zinkevych", IsOnline = true }
                    );
                _context.SaveChanges();
            }
        }
        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }

    }
}
