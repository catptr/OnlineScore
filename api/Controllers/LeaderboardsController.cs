using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScoreApi.Data;
using ScoreApi.Models;

namespace ScoreApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class LeaderboardsController : Controller
    {
        private readonly ScoreContext _context;

        public LeaderboardsController(ScoreContext context)
        {
            _context = context;
        }

        // GET api/leaderboards
        [HttpGet]
        public async Task<IEnumerable<Leaderboard>> GetLeaderboards()
        {
            return await _context.Leaderboards.ToListAsync();
        }

        // GET api/leaderboards/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScoresFromLeaderboard(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var leaderboard = await _context.Leaderboards
                .Include(l => l.Scores)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (leaderboard == null)
            {
                return NotFound();
            }

            return new ObjectResult(leaderboard.Scores.ToList());
        }

        // POST api/values
        [HttpPost("{id}")]
        public async Task<IActionResult> Create(int id, [FromBody][Bind("Name,Value")] Score score)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var leaderboardExists = await _context.Leaderboards.AnyAsync(l => l.Id == id);
            if (!leaderboardExists)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                score.LeaderboardId = id;

                // TEMPORARY HACK WHILE WE ARE USING IN-MEMORY DATABASE
                long maxId = await _context.Scores.Select(s => s.Id).MaxAsync();
                score.Id = maxId + 1;

                _context.Add(score);
                await _context.SaveChangesAsync();
                // Should return a RedirectToAction but only if we offer
                // getting a single score, which I don't think we will.
                return new NoContentResult();
            }

            return BadRequest();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
