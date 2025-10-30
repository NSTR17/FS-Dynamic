using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FS_Dynamic.Models;

namespace FS_Dynamic
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Обработчик запуска приложения - вызывается ПЕРВЫМ при старте
        /// Здесь решаем какое окно открывать
        /// </summary>
        /// 
        private void Application_Startup(object sender, StartupEventArgs e)
        { 
            var loginWindow = new LoginWindow();
            bool? loginResult = loginWindow.ShowDialog();

            if (loginResult == true && loginWindow.CurrentUser != null)
            {
                OpenMainWindowBasedOnRole(loginWindow.CurrentUser);
            }
            else
            { 
                Current.Shutdown();
            }
        }

        /// <summary>
        /// Открывает главное окно в зависимости от роли пользователя
        /// </summary>
        /// <param name="user">Авторизованный пользователь</param>
        /// 

        private void OpenMainWindowBasedOnRole(User user)
        {
            Window mainWindow;

            switch (user.Role.ToLower())
            {
                case "admin":
                    mainWindow = new MainWindow(user);
                    break;
                case "operator":
                    mainWindow = new OperatorWindow(user);
                    break;
                case "athlete":
                    mainWindow = new AthleteWindow(user);
                default:
                    MessageBox.Show($"Неизвестная роль: {user.Role}", "Ошибка");
                    Current.Shutdown();
                    return;
            }
            mainWindow.Show();
        }
    }
}
