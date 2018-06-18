using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanningPokerBackend.Models;
using PlanningPokerBackend.Models.PostRequestBodyModels;

namespace PlanningPokerBackend.Controllers
{
    public class PlayTablesController : Controller
    {
        private readonly PlanningPokerDbContext _context;

        public PlayTablesController(PlanningPokerDbContext context)
        {
            _context = context;
        }
        public IActionResult GetAll()
        {
            return new ObjectResult(_context.PlayTables.Include(pt => pt.Admin).Select((pt) => new { AdminId = pt.Admin.Id, Participants = pt.Participants.Select((p) => new { p.Id, p.FirstName, p.LastName }) }));
        }
        public IActionResult GetId(TokenBody body)
        {
            User user = _context.Users.Include(u => u.PlayTable).FirstOrDefault(u => u.Token == body.Token);
            if (body.Token == null || body.Token == "" || user == null)
            {
                return BadRequest("Wrong token");
            }
            if (user.PlayTable == null)
            {
                return BadRequest("You have no tables");
            }
            return new ObjectResult(user.PlayTable.Id);
        }
        [HttpPost]
        public IActionResult Create([FromBody] TokenBody body)
        {
            User user = _context.Users.FirstOrDefault(u => u.Token == body.Token);
            if (body.Token == null || body.Token == "" || user == null)
            {
                return BadRequest("Wrong token");
            }
            // One user - one table
            PlayTable playTable = _context.PlayTables.Include(pt => pt.Participants).Include(pt => pt.Admin).FirstOrDefault(pt => pt.Admin.Id == user.Id);
            if (playTable != null)
            {
                playTable.Participants.ToList().ForEach(p => p.PlayTable = null);
                _context.Remove(playTable);
            }
            var token = Guid.NewGuid().ToString("n").Substring(0, 5);
            user.PlayTable = new PlayTable() { Admin = user, Token = token };
            user.IsReady = true;
            _context.Update(user);
            _context.SaveChanges();
            return Ok(token);
        }
        [HttpDelete]
        public IActionResult Delete([FromBody] TokenBody body)
        {
            User user = _context.Users.FirstOrDefault(u => u.Token == body.Token);
            if (body.Token == null || body.Token == "" || user == null)
            {
                return BadRequest("Wrong token");
            }
            PlayTable playTable = _context.PlayTables.Include(pt => pt.Participants).Include(pt => pt.Admin).FirstOrDefault(pt => pt.Admin.Id == user.Id);
            if (playTable == null)
            {
                return BadRequest("You have no tables");
            }
            playTable.Participants.ToList().ForEach(p => p.PlayTable = null);
            _context.Update(playTable);
            _context.Remove(playTable);
            _context.SaveChanges();
            return Ok();
        }
        [HttpPost]
        public IActionResult Join([FromBody] UserTokenAndTableToken body)
        {
            User user = _context.Users.FirstOrDefault(u => u.Token == body.UserToken);
            if (body.UserToken == null || body.UserToken == "" || user == null)
            {
                return BadRequest("Wrong user token");
            }
            PlayTable playTable = _context.PlayTables.FirstOrDefault(pt => pt.Token == body.TableToken);
            if (playTable == null)
            {
                return BadRequest("Table not found");
            }
            playTable.Participants.Add(user);
            user.IsReady = false;
            _context.Update(playTable);
            _context.Update(user);
            _context.SaveChanges();
            return Ok();
        }
        public IActionResult GetParticipants(TokenBody body)
        {
            User user = _context.Users.FirstOrDefault(u => u.Token == body.Token);
            if (body.Token == null || body.Token == "" || user == null)
            {
                return BadRequest("Wrong token");
            }
            PlayTable playTable = _context.PlayTables.Include(pt => pt.Participants).FirstOrDefault(pt => pt.Participants.Any(u => u.Id == user.Id));
            if (playTable == null)
            {
                return BadRequest("You have no tables");
            }
            return new ObjectResult(playTable.Participants.Select((p) => new { p.Id, p.Email, p.FirstName, p.LastName, p.IsReady }));
        }
        [HttpPost]
        public IActionResult KickParticipant([FromBody] TokenAndEmailBody body)
        {
            User user = _context.Users.FirstOrDefault(u => u.Token == body.UserToken);
            if (body.UserToken == null || body.UserToken == "" || user == null)
            {
                return BadRequest("Wrong token");
            }
            User userToKick = _context.Users.FirstOrDefault(u => u.Email == body.ParticipantEmail);
            if (userToKick == null)
            {
                return BadRequest("Wrong e-mail of participant");
            }
            // Just admin can add new participants
            PlayTable playTable = _context.PlayTables.Include(pt => pt.Admin).FirstOrDefault(pt => pt.Admin.Id == user.Id);
            if (playTable == null)
            {
                return BadRequest("You have no tables");
            }
            if (playTable.Participants.Count(p => p.Id == userToKick.Id) == 0)
            {
                return BadRequest("User is not a participant of this table");
            }
            playTable.Participants.Remove(userToKick);
            userToKick.PlayTable = null;
            _context.Update(playTable);
            _context.Update(userToKick);
            _context.SaveChanges();
            return Ok();
        }
    }
}