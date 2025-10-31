using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using FS_Dynamic.Models;
using System.Data.Entity.Core;
using System.Web;

namespace FS_Dynamic.Services
{
    /// <summary>
    /// Сервис для взаимодействия с API авторизации
    /// Отвечает за отправку запросов на сервер и обработку ответов
    /// </summary>
    internal class AuthService
    {
        private readonly string _apiBaseUrl = "http://localhost/fs-dynamic/api/"; // Базовый URL нашего API
        private readonly HttpClient _httpClient;  // HttpClient - класс для отправки HTTP запросов

        public AuthService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMilliseconds(30); // максимальное время ожидания ответа от сервера
        }

        /// <summary>
        /// Асинхронный метод для авторизации пользователя
        /// Отправляет логин и пароль на сервер, возвращает результат
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>Объект AuthResponse с результатом авторизации</returns>
        /// 

        //public async Task<AuthResponse> LoginAsync(string login, string password)
        //{
        //    try
        //    {
        //        var requsestData = new // анонимный объект с данными для отправки
        //        {
        //            login = login,
        //            password = password
        //        };

        //        var json = JsonConvert.SerializeObject(requsestData); // Сериализуем анонимный объект в Json для PHP

        //        var content = new StringContent(json, Encoding.UTF8, "application/json"); // Создается контект для HTTP запрос

        //        var responce = await _httpClient.PostAsync(_apiBaseUrl + "auth.php", content);  // Отправляем POST запрос на сервер

        //        var responceJson = await responce.Content.ReadAsStringAsync(); //  POST ответ на запрос на сервер

        //        // Проврека статуса ответа

        //        if (responce.IsSuccessStatusCode)
        //        {
        //            var authResult = JsonConvert.DeserializeObject<AuthResponse>(responceJson); // Десериализация ответа из Json в объект AuthResponce

        //            return authResult;
        //        }
        //        else
        //        {
        //            // Если статус ответа не успешный, то создается новый объект с ошибкой
        //            return new AuthResponse
        //            {
        //                Success = false,
        //                Error = $"HTTP Error: {responce.StatusCode}"
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new AuthResponse
        //        {
        //            Success = false,
        //            Error = $"Ошибка подключения: {ex.Message}"
        //        };
        //    }
        //}

        public async Task<AuthResponse> LoginAsync(string login, string password)
        {
            // ВРЕМЕННАЯ ЗАГЛУШКА - закомментируйте когда API заработает
            return await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(500); // Имитация задержки

                if (login == "admin" && password == "password")
                {
                    return new AuthResponse
                    {
                        Success = true,
                        User = new User
                        {
                            Id = 1,
                            Login = "admin",
                            FullName = "Администратор Системы",
                            Role = "admin",
                            //Email = "admin@fsdynamic.ru"
                        }
                    };
                }
                else if (login == "operator1" && password == "password")
                {
                    return new AuthResponse
                    {
                        Success = true,
                        User = new User
                        {
                            Id = 2,
                            Login = "operator1",
                            FullName = "Иван Операторов",
                            Role = "operator",
                            //Email = "operator@fsdynamic.ru"
                        }
                    };
                }
                else if (login == "athlete1" && password == "password")
                {
                    return new AuthResponse
                    {
                        Success = true,
                        User = new User
                        {
                            Id = 3,
                            Login = "athlete1",
                            FullName = "Петр Спортсменов",
                            Role = "athlete",
                            //Email = "athlete@fsdynamic.ru"
                        }
                    };
                }
                else
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Error = "Неверный логин или пароль"
                    };
                }
            });

        }
    }
}
