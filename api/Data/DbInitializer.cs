using System;
using System.Linq;
using ScoreApi.Models;

namespace ScoreApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ScoreContext context)
        {
            if (context.Scores.Any())
            {
                return;
            }

            var leaderboards = new Leaderboard[]
            {
                new Leaderboard { Id = 1, Owner = 1 },
                new Leaderboard { Id = 2, Owner = 1 },
            };

            foreach (Leaderboard leaderboard in leaderboards)
            {
                context.Leaderboards.Add(leaderboard);
            }
            context.SaveChanges();

            var scores = new Score[]
            {
                new Score { LeaderboardId = 1, Id = 1, Name = "Mjau",      Value = 1337 },
                new Score { LeaderboardId = 1, Id = 2, Name = "BluePanda", Value = 999 },
                new Score { LeaderboardId = 1, Id = 3, Name = "Björnis",   Value = 500 },
                new Score { LeaderboardId = 2, Id = 4, Name = "Trassan",   Value = 631 },
                new Score { LeaderboardId = 2, Id = 5, Name = "harvest",   Value = 789 },
                new Score { LeaderboardId = 2, Id = 6, Name = "Levlette",  Value = 873 },
            };

            foreach (Score score in scores)
            {
                context.Scores.Add(score);
            }
            context.SaveChanges();
        }
    }
}