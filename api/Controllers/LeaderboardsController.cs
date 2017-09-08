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

        [HttpGet]
        public async Task<IEnumerable<Leaderboard>> GetLeaderboards()
        {
            return await _context.Leaderboards.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScoresFromLeaderboard(long id)
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

            return new JsonResult(leaderboard.Scores.ToList());
        }

        [HttpGet("{leaderId}")]
        public async Task<IActionResult> GetSingleScoreFromLeaderboard(long leaderId, [FromQuery]long id)
        {
            if (leaderId == 0)
            {
                return NotFound();
            }

            if (id == 0)
            {
                return NotFound();
            }

            var score = await _context.Scores.FirstOrDefaultAsync(s => s.LeaderboardId == leaderId && s.Id == id);
            if (score == null)
            {
                return NotFound();
            }

            return new JsonResult(score);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Create(long id, [FromBody]ScoreInputModel scoreInput)
        {
            // Nullable so that we can differentiate between none passed and zero.
            if (id == 0)
            {
                return NotFound();
            }

            var leaderboardExists = await _context.Leaderboards.AnyAsync(l => l.Id == id);
            if (!leaderboardExists)
            {
                return NotFound();
            }

            // TODO: More validation e.g. minimum length.
            if (scoreInput.Name == null)
            {
                return BadRequest();
            }

            // TODO: More validation e.g. allowed ranges.
            if (!scoreInput.Value.HasValue)
            {
                return BadRequest();
            }

            Score score = new Score { LeaderboardId = id, Name = scoreInput.Name, Value = scoreInput.Value.Value };
            // TEMPORARY HACK WHILE WE ARE USING IN-MEMORY DATABASE
            long maxId = await _context.Scores.Select(s => s.Id).MaxAsync();
            score.Id = maxId + 1;

            _context.Add(score);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        [HttpDelete("{leaderId}")]
        public async Task<IActionResult> Delete(long leaderId, [FromQuery]long id)
        {
            if (leaderId == 0)
            {
                return NotFound();
            }

            if (id == 0)
            {
                return NotFound();
            }

            var score = await _context.Scores.SingleOrDefaultAsync(s => s.LeaderboardId == leaderId && s.Id == id);
            if (score == null)
            {
                // Should we check leaderboard id separately for error message sake?
                // Though we don't even have error messages here lul.
                return NotFound();
            }

            // Does _context also have a Remove method and if so which should we use?
            _context.Scores.Remove(score);
            await _context.SaveChangesAsync();

            // What should we return?
            return new NoContentResult();
        }
    }
}
