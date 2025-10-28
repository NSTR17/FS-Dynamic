using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;
using System.Timers;
using System.Data.SQLite;
using System.Data.Entity;
using System.Windows.Threading;


namespace FS_Dynamic
{
    /// <summary>
    /// Логика взаимодействия для DemoWindow.xaml
    /// </summary>
    public partial class DemoWindow : Window
    {
        private DispatcherTimer updateTimer;
        private MainWindow mainWindow;


        public DemoWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
                       
            InitializeDemoDisplay();

            if (mainWindow != null) // Подписываемся на события обновления данных
            {
                mainWindow.DataUpdated += OnMainWindowDataUpdated;
            }

            
        }
                       

        private void InitializeDemoDisplay()
        {
            UpdateAllData();  // Первоначальная настройка данных

            updateTimer = new DispatcherTimer(); //Таймер для периодического обновления
            updateTimer.Interval = TimeSpan.FromMilliseconds(100);
            updateTimer.Tick += (s, e) => UpdateAllData();
            updateTimer.Start();

        }

        private void OnMainWindowDataUpdated() // Обновляем при изменении данных в основном окне
        {
            Dispatcher.BeginInvoke(new Action(UpdateAllData));
        }

        private void UpdateAllData()
        {
            // Мгновенное обновление при изменении данных в основном окне
           
            try
            {
                if (mainWindow != null)
                {
                    Result_Demo.Text = mainWindow.TimeValue;
                    Result_plus_Busts.Text = mainWindow.FinalTimeValue;
                    Bust_Q.Text = mainWindow.BustValue;
                    Skip_Q.Text = mainWindow.SkipValue;
                    Team_Name.Text = mainWindow.SelectedTeam;
                    Round_Number.Text = mainWindow.SelectedRound;

                }
                else
                {
                    updateTimer?.Stop();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка обновления Demo окна: {ex.Message}");
            }


        }

        protected override void OnClosed(EventArgs e)
        {
            if (mainWindow != null) 
            {
                mainWindow.DataUpdated -= OnMainWindowDataUpdated;
            }
            updateTimer?.Stop();
            base.OnClosed(e);
        }

    }
}
