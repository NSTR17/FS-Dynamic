using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FS_Dynamic.Models
{
    public class User
    {
        /// <summary>
        /// Класс для представления пользователя системы
        /// Содержит все свойства, которые получаем из API
        /// </summary>
        /// 
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }


        public DateTime CreatedAt { get; set; }

    }
}
