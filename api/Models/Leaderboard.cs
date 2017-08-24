using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ScoreApi.Models
{
    public class Leaderboard
    {
        public long Id { get; set; }
        // Will match owner id in identityserver later
        public long Owner { get; set; }

        // Maybe this can be removed if we decide to just not make this model public
        [JsonIgnore]
        public ICollection<Score> Scores { get; set; }
    }
}