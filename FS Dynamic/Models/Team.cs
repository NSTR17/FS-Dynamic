using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS_Dynamic.Models
{
    public class Team
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("disciplines")]
        public string Disciplines { get; set; }

        [JsonProperty("athlete_id")]
        public int AthleteId { get; set; }
    }
}
