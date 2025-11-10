using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS_Dynamic.Models
{
    public class TeamResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("teams")]
        public List<Team> Teams { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
