using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ScoreApi.Models
{
    public class Score
    {
        public long Id { get; set; }
        
        // This is needed by us but not by consumers of the api
        [JsonIgnore]
        public long LeaderboardId { get; set; }
        public String Name { get; set; }
        // If someone wants floats, let's use fixed point lul. For now at least.
        public long Value { get; set; }

        // Keep this property or not?
        [JsonIgnore]
        public Leaderboard Leaderboard { get; set; }
    }
}