using FS_Dynamic.Models;
using FS_Dynamic.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
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
using System.Windows.Threading;


namespace FS_Dynamic
{
    /// <summary>
    /// Логика взаимодействия для OperatorWindow.xaml
    /// </summary>
    public partial class OperatorWindow : Window
    {
        
        SerialPort sp = new SerialPort();
        string[] ports = SerialPort.GetPortNames();
        DispatcherTimer decorativeTimer;
        Stopwatch stopWatch = new Stopwatch();
        TimeSpan ts = new TimeSpan();
        TimeSpan ts_0 = new TimeSpan();
        int off_Quantity_Value = 2;
        int off_Quantity_Counter = 0;
        string res;
        string set = "Set";
        private TeamService _teamService;
        private ResultService _resultService;
        private List<Team> _allTeams;
        private Team _selectedTeam;

        public OperatorWindow()
        {
            InitializeComponent();
            _teamService = new TeamService();
            _resultService = new ResultService();
            InitializeDecorativeTimer();
            COM.ItemsSource = ports;
            sp.DataReceived += new SerialDataReceivedEventHandler(DataReceived);

            LoadDisciplines();
            
        }
                
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Dispatcher.Invoke(() => TextIn.Text = res = sp.ReadExisting());
           
            
                switch (res)
                {
                    case "on":
                        stopWatch.Start();
                        StartDecorativeTimer();

                        break;
                    case "off":
                        TimeSpan ts = stopWatch.Elapsed;
                        ts_0 = ts;
                        off_Quantity_Counter++;
                        if (off_Quantity_Counter == off_Quantity_Value)
                        {
                            StopDecorativeTimer();
                        }
                        break;

                }
            

        }

        private void InitializeDecorativeTimer() // Инициализация декоративного таймера
        {
            decorativeTimer = new DispatcherTimer();
            decorativeTimer.Interval = TimeSpan.FromMilliseconds(30);
            decorativeTimer.Tick += (s, e) => UpdateDecorativeDisplay();
        }

        private void StartDecorativeTimer() // Запуск декоративного таймера
        {
            decorativeTimer.Start();
        }

        private void StopDecorativeTimer() // Останов декоративного таймера
        {
            decorativeTimer?.Stop();
        }

        private void UpdateDecorativeDisplay() // Вывод данных декоративного таймера в UI
        {
            if (stopWatch.IsRunning)
            {
                TimeSpan rs = stopWatch.Elapsed;
                string elapsedTime = $"{(int)rs.TotalSeconds:00}:{rs.Milliseconds:000}";

                txtStopWatch.Text = elapsedTime; // Декоративное отображение
                
            }
        }


        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                stopWatch.Stop();
                string elapsedTime = String.Format("{0:00}:{1:000}", (int)ts_0.TotalSeconds, ts_0.Milliseconds);
                Dispatcher.Invoke(() => txtPrevTime.Text = elapsedTime);
                Dispatcher.Invoke(() => txtStopWatch.Text = "00:000");
                await SaveTrainingResult();
                stopWatch.Reset();
                off_Quantity_Counter = 0;
                sp.Write("f");
                System.Threading.Thread.Sleep(500);
                sp.Write("y");
                System.Threading.Thread.Sleep(500);
                sp.Write("w");
                Dispatcher.Invoke(() => TextIn.Text = set);

            }
            else
            {
                sp.Write("y");
                System.Threading.Thread.Sleep(500);
                sp.Write("w");
                Refresh.Content = "Restart";
                Dispatcher.Invoke(() => TextIn.Text = set);
            }
        }

        private void BtnLines_Click(object sender, RoutedEventArgs e)
        {
            sp.Write("f");
            sp.Write("g");
            Refresh.Content = "Start";
        }

        private void COM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (sp.IsOpen)
                {
                    sp.Close();
                }
                sp.PortName = COM.SelectedItem as string;
                sp.BaudRate = 250000;
                sp.Open();
                MessageBox.Show("Порт открыт");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
               

        private async void Discipline_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Team_Name.SelectedItem = null;
            if (Discipline.SelectedItem != null)
            {
                string selectedDiscipline = Discipline.SelectedItem as string;


                Team_Name.IsEnabled = true;
                Team_Name.ItemsSource = new List<string> { "Загрузка..." };

                await LoadTeamByDiscipline(selectedDiscipline);
            }
            else 
            {
                Team_Name.IsEnabled = false;
                Team_Name.ItemsSource = null;
                _selectedTeam = null;
            }
        }

        private async Task LoadTeamByDiscipline(string discipline)
        {
            try
            {
                var teams = await _teamService.GetTeamsAsync(discipline);

                if (teams.Any())
                {
                    Team_Name.ItemsSource = teams;
                    Team_Name.DisplayMemberPath = "Name";
                }
                else
                {
                    Team_Name.ItemsSource = new List<string> { "Нет команд" };
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки команд: {ex.Message}");
            }
        }

        private void LoadDisciplines()
        { 
            var disciplines = new List<string> { "DS", "D2W", "D4W" };
            Discipline.ItemsSource = disciplines;

            Team_Name.IsEnabled = false;
            Team_Name.ItemsSource = null;
        }
               

        private void Team_Name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Team_Name.SelectedItem is Team team)
            {
                _selectedTeam = team;
            }
            else 
            {
                _selectedTeam = null;
            }
        }

        //private async Task SaveTrainigResult()
        //{
        //    if (_selectedTeam == null || Discipline.SelectedItem == null)
        //    {
        //        MessageBox.Show("Выберите дисциплину и команду перед сохранением");
        //    }
        //    try
        //    {
        //        var result = new TrainingResult
        //        {
        //            TeamId = _selectedTeam.Id,
        //            AthleteId = _selectedTeam.AthleteId,
        //            Discipline = Discipline.SelectedItem.ToString(),
        //            TimeMs = (int)ts_0.TotalMilliseconds,
        //            Busts = 0,
        //            Skips = 0
        //        };

        //        bool succes = await _resultService.SaveTrainingResultAsync(result);

        //        if (succes)
        //        {
        //            MessageBox.Show("Результат успешно сохранен");
        //        }
        //        else
        //        {
        //            MessageBox.Show("Ошибка сохранения результата");
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка {ex.Message}");
        //    }
        //}

        private async Task SaveTrainingResult()
        {
            System.Diagnostics.Debug.WriteLine("=== SAVE TRAINING RESULT METHOD ===");

            if (_selectedTeam == null)
            {
                System.Diagnostics.Debug.WriteLine("Selected team is NULL");
                MessageBox.Show("Выберите команду перед сохранением");
                return;
            }

            if (Discipline.SelectedItem == null)
            {
                System.Diagnostics.Debug.WriteLine("Discipline is NULL");
                MessageBox.Show("Выберите дисциплину перед сохранением");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Selected Team: {_selectedTeam.Id}, Athlete: {_selectedTeam.AthleteId}");
            System.Diagnostics.Debug.WriteLine($"Discipline: {Discipline.SelectedItem}");
            System.Diagnostics.Debug.WriteLine($"Time: {ts_0.TotalMilliseconds}ms");

            try
            {
                var result = new TrainingResult
                {
                    TeamId = _selectedTeam.Id,
                    AthleteId = _selectedTeam.AthleteId,
                    Discipline = Discipline.SelectedItem.ToString(),
                    TimeMs = (int)ts_0.TotalMilliseconds,
                    Busts = 0,
                    Skips = 0
                };

                bool success = await _resultService.SaveTrainingResultAsync(result);

                if (success)
                {
                    MessageBox.Show("Результат успешно сохранен!");
                }
                else
                {
                    MessageBox.Show("Ошибка сохранения результата");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Save Exception: {ex}");
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}

