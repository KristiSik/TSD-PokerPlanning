using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanningPokerBackend.Models;
using PlanningPokerBackend.Models.PostRequestBodyModels;

namespace PlanningPokerBackend.Controllers
{
    public class GamesController : Controller
    {
        private readonly PlanningPokerDbContext _context;

        public GamesController(PlanningPokerDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult Start([FromBody] TokenBody body)
        {
            User user = _context.Users.Include(u => u.PlayTable).FirstOrDefault(u => u.Token == body.Token);
            if (body.Token == null || body.Token == "" || user == null)
            {
                return BadRequest("Wrong token");
            }
            PlayTable playTable = _context.PlayTables.Include(pt => pt.Admin).Include(pt => pt.CurrentGame).Include(pt => pt.Participants).FirstOrDefault(pt => pt == user.PlayTable);
            if (playTable.Admin != user)
            {
                return BadRequest("Only admin can start new game");
            }
            foreach(var participant in playTable.Participants.ToList())
            {
                if (participant.IsReady == false)
                {
                    return BadRequest("Not everyone is ready");
                }
                participant.IsReady = false;
            }
            if (playTable.CurrentGame != null)
            {
                playTable.CurrentGame.IsFinished = true;
                _context.Update(playTable.CurrentGame);
            }
            playTable.CurrentGame = new Game();
            _context.Update(user);
            _context.SaveChanges();
            return Ok();
        }
        public IActionResult IsStarted(TokenBody body)
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
            Game game = _context.Games.Include(g => g.PlayTable).FirstOrDefault(g => g.PlayTable == user.PlayTable && g.IsFinished == false);
            if (game != null)
            {
                return Ok("true");
            }
            return Ok("false");
        }
        [HttpPost]
        public IActionResult SendAnswer([FromBody] TokenAndAnswerBody body)
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
            Game game = _context.Games.Include(g => g.PlayTable).ThenInclude(pt => pt.Participants).Include(g => g.Answers).FirstOrDefault(g => g.PlayTable == user.PlayTable && g.IsFinished == false);
            if (game == null)
            {
                return BadRequest("Game not started or is already finished");
            }
            if (string.IsNullOrEmpty(body.Answer))
            {
                return BadRequest("Answer can't be empty");
            }
            Answer usersAnswer = game.Answers.FirstOrDefault(a => a.User == user);
            if (usersAnswer != null)
            {
                usersAnswer.Value = body.Answer;
                _context.Update(usersAnswer);
            }
            else
            {
                game.Answers.Add(new Answer() { User = user, Value = body.Answer });
                _context.Update(game);
            }
            _context.SaveChanges();
            return Ok();
        }
        public IActionResult GetResults(TokenBody body)
        {
            User user = _context.Users.Include(u => u.PlayTable).FirstOrDefault(u => u.Token == body.Token);
            if (body.Token == null || body.Token == "" || user == null)
            {
                return BadRequest("Wrong token" + body.Token);
            }
            if (user.PlayTable == null)
            {
                return BadRequest("You have no tables");
            }
            Game game = _context.Games.Include(g => g.PlayTable).Include(g => g.Answers).FirstOrDefault(g => g.PlayTable == user.PlayTable);
            if (game == null)
            {
                return BadRequest("Game not found");
            }
            else
            if (game.IsFinished == false)
            {
                return BadRequest("Game is not finished yet");
            }
            var answers = new List<Answer>();
            foreach(var answer in game.Answers)
            {
                answers.Add(_context.Answers.Include(a => a.User).First(a => a.Id == answer.Id));
            }
            return new ObjectResult(answers.Select(a => new { User = new { a.User.Email, a.User.FirstName, a.User.LastName }, a.Value }).OrderBy(a => a.Value));
        }
        [HttpPost]
        public IActionResult SetReadyStatus([FromBody] TokenAndIsReadyStatusBody body)
        {
            User user = _context.Users.Include(u => u.PlayTable).FirstOrDefault(u => u.Token == body.UserToken);
            if (body.UserToken == null || body.UserToken == "" || user == null)
            {
                return BadRequest("Wrong token");
            }
            if (user.PlayTable == null)
            {
                return BadRequest("You have no tables");
            }
            if (body.IsReady == null)
            {
                return BadRequest("IsReady field is empty");
            }

            PlayTable playTable = _context.PlayTables.Include(pt => pt.Admin).Include(pt => pt.CurrentGame).Include(pt => pt.Participants).FirstOrDefault(pt => pt.Id == user.PlayTable.Id);
            if (playTable.CurrentGame == null)
            {
                return BadRequest("Game not started or is already finished");
            }
            Game game = _context.Games.Include(g => g.Answers).FirstOrDefault(g => g.Id == playTable.CurrentGame.Id);
            Answer answer = game.Answers.FirstOrDefault(a => a.User.Id == user.Id);
            if (body.IsReady == false)
            {
                if (answer != null)
                {
                    _context.Entry(answer).State = EntityState.Deleted;
                }
            } else
            {
                if (answer == null)
                {
                    return BadRequest("You didn't send answer");
                }
            }
            user.IsReady = body.IsReady ?? false;
            _context.Update(user);
            bool everyOneIsReady = true;
            foreach (var participant in playTable.Participants) {
                if (!participant.IsReady)
                {
                    everyOneIsReady = false;
                    break;
                }
            }
            if (everyOneIsReady)
            {
                playTable.CurrentGame.IsFinished = true;
                playTable.CurrentTaskName = null;
                _context.Update(playTable);
            }
            _context.SaveChanges();
            return Ok();
        }
        public IActionResult IsFinished(TokenBody body)
        {
            User user = _context.Users.Include(u => u.PlayTable).ThenInclude(pt => pt.CurrentGame).FirstOrDefault(u => u.Token == body.Token);
            if (body.Token == null || body.Token == "" || user == null)
            {
                return BadRequest("Wrong token");
            }
            if (user.PlayTable == null)
            {
                return BadRequest("You have no tables");
            }
            if (user.PlayTable.CurrentGame != null)
            {
                return Ok(user.PlayTable.CurrentGame.IsFinished);
            } else
            {
                return BadRequest("Game is not started");
            }
        }
    }
}