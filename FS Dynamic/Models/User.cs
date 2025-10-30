using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS_Dynamic.Models
{
    public class User
    {
        /// <summary>
        /// Класс для представления пользователя системы
        /// Содержит все свойства, которые получаем из API
        /// </summary>
        public int Id { get; set; }
        public string Login { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
