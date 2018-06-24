using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanningPokerBackend.Models;
using PlanningPokerBackend.Models.PostRequestBodyModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanningPokerBackend.Controllers
{
    public class UsersController : Controller
    {
        private readonly PlanningPokerDbContext _context;

        public UsersController(PlanningPokerDbContext context)
        {
            _context = context;
        }
        public IEnumerable<User> GetAll()
        {
            var l = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes("passwordsalt")));
            return _context.Users.Include(u => u.PlayTable).ToList();
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            return new ObjectResult(user);
        }
        [HttpPost]
        public IActionResult Add([FromBody] AddUser user)
        {
            //TODO: email regex
            //TODO: password salt
            if (user != null &
                !String.IsNullOrEmpty(user.FirstName) &
                !String.IsNullOrEmpty(user.LastName) &
                !String.IsNullOrEmpty(user.Email) &
                !String.IsNullOrEmpty(user.Password) &
                user.FirstName.Length >= 3 &
                user.LastName.Length >=3 &
                user.Email.Length >= 4 &
                user.Password.Length >= 4)
            {
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    return BadRequest("Email already used");
                }
                string salt = GenerateSalt();
                _context.Users.Add(new User()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Password = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(user.Password + salt))),
                        Salt = salt
                    });
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginUser user)
        {
            User loginUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (loginUser == null || Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(user.Password + loginUser.Salt))) != loginUser.Password)
            {
                return BadRequest("Wrong login/password");
            } else
            {
                string token = GenerateToken();
                loginUser.Token = token;
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
                _context.Users.Update(logoutUser);
                _context.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        public IActionResult SetReadyStatus([FromBody] TokenAndIsReadyStatusBody body)
        {
            User user = _context.Users.FirstOrDefault(u => u.Token == body.UserToken);
            if (body.UserToken == null || body.UserToken == "" || user == null)
            {
                return BadRequest("Wrong token");
            }
            if (body.IsReady == null)
            {
                return BadRequest("IsReady field is empty");
            }
            user.IsReady = body.IsReady??false;
            _context.Update(user);
            _context.SaveChanges();
            return Ok();
        }
        private string GenerateToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("+", "").Replace("=", "");
        }
        private string GenerateSalt()
        {
            const string valid = "1234567890qwertyuiopasdfghjklzxcvbnmQAZWSXEDCRFVTGBYHNUJMIKOLP";
            StringBuilder salt = new StringBuilder();
            int length = new Random().Next(12, 36);
            using (RNGCryptoServiceProvider rngC = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];
                while (length-- > 0)
                {
                    rngC.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    salt.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return salt.ToString();
        }
    }
}