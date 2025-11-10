using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FS_Dynamic.Models
{
    /// <summary>
    /// Класс для представления ответа от API авторизации
    /// Содержит результат попытки входа и данные пользователя
    /// </summary>
    internal class AuthResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; } // Успешна ли авторизация (true/false)
        [JsonProperty("User")]  // Заглавная U как в PHP
        public User User { get; set; } // Данные пользователя, если авторизация успешна
        [JsonProperty("error")]  // строчная e как в PHP  
        public string Error { get; set; } // Сообщение об ошибке, если авторизация не удалась
        [JsonProperty("message")]  // строчная m как в PHP
        public string Message { get; set; } // Дополнительное сообщение от сервера
    }
}
