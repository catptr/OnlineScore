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
    public class ScoresController : Controller
    {
        private readonly ScoreContext _context;

        public ScoresController(ScoreContext context)
        {
            _context = context;
        }

        [HttpGet("{id}", Name = "GetScore")]
        public async Task<IActionResult> GetScore(long id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            // TODO: See if the user has access to this score's leaderboard.
            var score = await _context.Scores.FirstOrDefaultAsync(s => s.Id == id);

            if (score == null)
            {
                return NotFound();
            }

            return new JsonResult(score);
        }
    }
}
