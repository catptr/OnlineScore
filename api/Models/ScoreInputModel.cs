using System;

namespace ScoreApi.Models
{
    public class ScoreInputModel
    {
        // Has to match fields in Score
        public String Name { get; set; }
        public long? Value { get; set; }
    }
}