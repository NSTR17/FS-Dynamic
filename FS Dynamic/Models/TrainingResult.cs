using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS_Dynamic.Models
{
    public class TrainingResult
    {
        [JsonProperty("team_id")]
        public int TeamId { get; set; }

        [JsonProperty("athlete_id")]
        public int AthleteId { get; set; }

        [JsonProperty("discipline")]
        public string Discipline { get; set; }

        [JsonProperty("time_ms")]
        public int TimeMs { get; set; }

        [JsonProperty("busts")]
        public int Busts { get; set; }

        [JsonProperty("skips")]
        public int Skips { get; set; }

        
    }
}
