using System;
using Microsoft.EntityFrameworkCore;
using ScoreApi.Models;

namespace ScoreApi.Data
{
    public class ScoreContext : DbContext
    {
        public ScoreContext(DbContextOptions<ScoreContext> options)
            : base(options)
        {
        }

        public DbSet<Score> Scores { get; set; }
        public DbSet<Leaderboard> Leaderboards { get; set; }
    }
}