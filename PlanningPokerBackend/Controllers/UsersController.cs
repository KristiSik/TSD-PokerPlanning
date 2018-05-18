using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlanningPokerBackend.Models;
using PlanningPokerBackend.Models.PostRequestBodyModels;

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
        [HttpPost]
        public IActionResult Add([FromBody] AddUser user)
        {
            //TODO: email regex 
            if (user.FirstName.Length >= 3 &
                user.LastName.Length >=3 &
                user.Email.Length >= 4 &
                user.Password.Length >= 4)
            {
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    return BadRequest("Email already used");
                }
                _context.Users.Add(new User()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Password = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(user.Password)))
                    });
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginUser user)
        {
            string passwordSHA = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(user.Password)));
            User loginUser = _context.Users.FirstOrDefault(u => u.Email == user.Email & u.Password == passwordSHA);
            if (loginUser == null)
            {
                return BadRequest();
            } else
            {
                string token = GenerateToken();
                loginUser.Token = token;
                loginUser.IsOnline = true;
                _context.Users.Update(loginUser);
                _context.SaveChanges();
                return Ok(token);
            }
        }
        [HttpPost]
        public IActionResult Logout([FromBody] LogoutUser user)
        {
            User logoutUser = _context.Users.FirstOrDefault(u => u.Token == user.Token);
            if (logoutUser != null)
            {
                logoutUser.Token = "";
                logoutUser.IsOnline = false;
                _context.Users.Update(logoutUser);
                _context.SaveChanges();
            }
            return Ok();
        }
        private string GenerateToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("+", "").Replace("=", "");
        }
    }
}