using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS_Dynamic.Models
{
    /// <summary>
    /// Класс для представления ответа от API авторизации
    /// Содержит результат попытки входа и данные пользователя
    /// </summary>
    internal class AuthResponse
    {
        public bool Success { get; set; } // Успешна ли авторизация (true/false)
        public User User { get; set; } // Данные пользователя, если авторизация успешна
        public string Error { get; set; } // Сообщение об ошибке, если авторизация не удалась
        public string Messge { get; set; } // Дополнительное сообщение от сервера
    }
}
