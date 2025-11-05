using FS_Dynamic.Models;
using FS_Dynamic.Services;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace FS_Dynamic
{
    /// <summary>
    /// Окно авторизации пользователя
    /// Появляется при запуске приложения, запрашивает логин и пароль
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly AuthService _authService; // Сервис для работы с API
        public User CurrentUser { get; private set; } // Свойство для хранения данных авторизованного пользователя
        
        /// <summary>
        /// Конструктор окна - вызывается при создании окна
        /// </summary>
        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Войти"
        /// async - метод асинхронный (не блокирует интерфейс)
        /// </summary>
        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {

            var login = txtLogin.Text.Trim();
            var password = txtPassword.Password;

            System.Diagnostics.Debug.WriteLine("=== Login Attempt ===");
            System.Diagnostics.Debug.WriteLine($"Login: '{txtLogin.Text}', Password: '{txtPassword.Password}'");

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ShowError("Введите логин и пароль");
                return;
            }
            SetLoadingState(true);

            try
            {
                var result = await _authService.LoginAsync(login, password);

                if (result.Success)
                {
                    CurrentUser = result.User;
                    this.DialogResult = true;

                    Close();
                }
                else
                {
                    ShowError(result.Error ?? "Ошибка авторизации");
                }
            }
            catch (System.Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }



        /// <summary>
        /// Управляет состоянием элементов при загрузке
        /// </summary>
        /// <param name="isLoading">true - показываем загрузку, false - скрываем</param>
        /// 

        private void SetLoadingState(bool isLoading)
        { 
            btnLogin.IsEnabled = !isLoading; // Блокируем или разблокируем кнопку входа
            progressBar.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;

            txtLogin.IsEnabled = !isLoading;
            txtPassword.IsEnabled = !isLoading;
        }
        /// <summary>
        /// Показывает сообщение об ошибке
        /// </summary>
        /// <param name="message">Текст ошибки</param>
        /// 
        private void ShowError(string message)
        { 
            txtStatus.Text = message;
            txtStatus.Visibility = Visibility.Visible;
        }
    }
}
