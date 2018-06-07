﻿using System;
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
            User user = _context.Users.FirstOrDefault(u => u.Token == body.Token);
            if (body.Token == null || body.Token == "" || user == null)
            {
                return BadRequest("Wrong token");
            }
            PlayTable playTable = _context.PlayTables.Include(pt => pt.Admin).Include(pt => pt.CurrentGame).FirstOrDefault(pt => pt.Admin.Id == user.Id);
            if (playTable == null)
            {
                return BadRequest("Only admin can start new game");
            }
            if (playTable.CurrentGame != null)
            {
                playTable.CurrentGame.IsFinished = true;
                _context.Update(playTable.CurrentGame);
            }
            user.PlayTable.CurrentGame = new Game();
            _context.Update(user);
            _context.SaveChanges();
            return Ok();
        }
        public IActionResult IsStarted([FromBody] TokenBody body)
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
            Game game = _context.Games.Include(g => g.PlayTable).Include(g => g.Answers).FirstOrDefault(g => g.PlayTable == user.PlayTable && g.IsFinished == false);
            if (game == null)
            {
                return BadRequest("Game not started or is already finished");
            }
            if (game.Answers.Any(a => a.User == user))
            {
                return BadRequest("You already sent an answer");
            }
            if (string.IsNullOrEmpty(body.Answer))
            {
                return BadRequest("Answer can't be empty");
            }
            game.Answers.Add(new Answer() { User = user, Value = body.Answer });
            _context.Update(game);
            _context.SaveChanges();
            return Ok();
        }
    }
}