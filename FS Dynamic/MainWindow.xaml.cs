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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.IO;
using System.Data;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Threading;
using System.Timers;

namespace FS_Dynamic
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        SerialPort sp = new SerialPort();
        string[] ports = SerialPort.GetPortNames();
        Stopwatch stopWatch = new Stopwatch();
        string res;
        int bust_q = 0;
        int skip_q = 0;
        TimeSpan ts = new TimeSpan(0, 0, 0, 0, 0);
        TimeSpan ts_0 = new TimeSpan(0, 0, 0, 0, 0);

        string resultwithbusts;
        DateTime timer_1 = new DateTime(0, 0);
        DateTime timer_2 = new DateTime(0, 0);
        bool st_timer = false;
        AutoResetEvent stop_timer = new AutoResetEvent(false);
        string path = "C:\\+\\FS_Arduino\\Results.txt";
        string path_teams = "C:\\+\\FS_Arduino\\Teams.txt";
        string path_each_tuch = "C:\\+\\FS_Arduino\\Result_each_tuch.txt";
        string team_name;
        string round_number;
        string path_rounds = "C:\\+\\FS_Arduino\\Rounds.txt";
        int off_q = 0;
        string ready = "Ready";
        string set = "Set";
        TimeSpan interval = new TimeSpan(0, 0, 0, 0, 1);
        const int FlagOn = 1;
        const int FlagOff = 0;
        

        // Ниже свойства для доступа из DemoWindow
        public string TimeValue => Result.Text;
        public string FinalTimeValue => Result_plus_Busts.Text;
        public string BustValue => Bust_Q.Text;
        public string SkipValue => Skip_Q.Text;
        public string SelectedTeam => Team_Name.SelectedItem?.ToString() ?? "Команда не выбрана";
        public string SelectedRound => Rounds.SelectedItem?.ToString() ?? "Не выбран";

        public event Action DataUpdated; // Событие для уведомления об изменении данных

        private DispatcherTimer decorativeTimer;



        public MainWindow()
        {
            InitializeComponent();
            InitializeDecorativeTimer();

            
            COM.ItemsSource = ports;
            sp.DataReceived += new SerialDataReceivedEventHandler(DataRecieved);
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

        void DataRecieved(object sender, SerialDataReceivedEventArgs e) //Обработка входящих сигналов
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
                    Print();
                    ts_0 = ts;
                    off_q++;
                    if (off_q == 2)
                    {
                        StopDecorativeTimer();
                                         
                       
                    }
                    break;
            }

        }

       
        private void Yellow(object sender, RoutedEventArgs e) // Режим желтых линий
        {
            sp.Write("y");
            Dispatcher.Invoke(() => TextIn.Text = ready);
            Result.Text = "00:00:000";
            Result_plus_Busts.Text = "00:00:000";
            Bust_Q.Text = "0";
            Skip_Q.Text = "0";
            bust_q = 0;
            skip_q = 0;
            off_q = 0;
            Team_Name.SelectedIndex++;
            Data.Choosen_TeamName = Team_Name.Text;
        }

        private void White(object sender, RoutedEventArgs e)
        {
            sp.Write("w");
            Dispatcher.Invoke(() => TextIn.Text = set);
        }



        private void Bust_Click(object sender, RoutedEventArgs e) //Добавление баста
        {
            bust_q++;
            Bust_Q.Text = bust_q.ToString();
            sp.Write("b");
            OnDataUpdated();
        }

        private void Bust_min_Click(object sender, RoutedEventArgs e) //Снятие баста
        {
            bust_q--;
            Bust_Q.Text = bust_q.ToString();
            OnDataUpdated();
        }

        private void Skip_plus_Click(object sender, RoutedEventArgs e) //Добавление скипа
        {
            skip_q++;
            Skip_Q.Text = skip_q.ToString();
            sp.Write("b");
            OnDataUpdated();
        }

        private void Skip_min_Click(object sender, RoutedEventArgs e) //Снятие скипа
        {
            skip_q--;
            Skip_Q.Text = skip_q.ToString();
            OnDataUpdated();
        }

        private void Result_Time_Click(object sender, RoutedEventArgs e)
        {// Финализация и сохранение результатов

            TimeSpan ts_bust = new TimeSpan(0, 0, 0, 5, 0);
            TimeSpan ts_skip = new TimeSpan(0, 0, 0, 20, 0);
            if (bust_q != 0 && skip_q == 0)
            {// Басты есть, скипов нет

                TimeSpan ts_bust_v = TimeSpan.FromSeconds(ts_bust.Seconds * (bust_q));
                TimeSpan overall = ts_bust_v.Add(ts_0);
                string time = $"{(int)ts_0.TotalSeconds:00}:{ts_0.Milliseconds:000}";
                resultwithbusts = $"{(int)overall.TotalSeconds:00}:{overall.Milliseconds:000}";

                Result_plus_Busts.Text = resultwithbusts;
                team_name = Team_Name.Text;
                round_number = Rounds.Text;
                string Bust_q = bust_q.ToString();
                string Space = " Busts: ";
                string OverAllResult = round_number + team_name + resultwithbusts + Space + Bust_q;
                FileStream file = new FileStream(path, FileMode.Append);
                StreamWriter stream = new StreamWriter(file);
                stream.WriteLine(OverAllResult);
                stream.Close();
                file.Close();
                               
                OnDataUpdated();

            }
            else if (bust_q == 0 && skip_q != 0)
            {// Бастов нет, скипы есть

                TimeSpan ts_skip_v = TimeSpan.FromSeconds(ts_skip.Seconds * (skip_q));
                TimeSpan overall = ts_skip_v.Add(ts_0);
                string time = $"{(int)ts_0.TotalSeconds:00}:{ts_0.Milliseconds:000}";
                resultwithbusts = $"{(int)overall.TotalSeconds:00}:{overall.Milliseconds:000}";
                Result_plus_Busts.Text = resultwithbusts;
                team_name = Team_Name.Text;
                round_number = Rounds.Text;
                string Skip_q = skip_q.ToString();
                string Space = " Skip: ";
                string OverAllResult = round_number + team_name + resultwithbusts + Space + Skip_q;
                FileStream file = new FileStream(path, FileMode.Append);
                StreamWriter stream = new StreamWriter(file);
                stream.WriteLine(OverAllResult);
                stream.Close();
                file.Close();
               
                OnDataUpdated();
            }
            else if (bust_q != 0 && skip_q != 0)
            { //Басты и скипы есть
                TimeSpan ts_skip_v = TimeSpan.FromSeconds(ts_skip.Seconds * (skip_q));
                TimeSpan ts_bust_v = TimeSpan.FromSeconds(ts_bust.Seconds * (bust_q));
                TimeSpan preview_overall = ts_bust_v.Add(ts_0);
                TimeSpan overall = preview_overall.Add(ts_skip_v);
                string time = $"{(int)ts_0.TotalSeconds:00}:{ts_0.Milliseconds:000}";
                resultwithbusts = $"{(int)overall.TotalSeconds:00}:{overall.Milliseconds:000}";
                Result_plus_Busts.Text = resultwithbusts;
                team_name = Team_Name.Text;
                round_number = Rounds.Text;
                string Skip_q = skip_q.ToString();
                string Space = " Skip: ";
                string Bust_q = bust_q.ToString();
                string Space_1 = " Busts: ";
                string OverAllResult = round_number + team_name + resultwithbusts + Space_1 + Bust_q + Space + Skip_q;
                FileStream file = new FileStream(path, FileMode.Append);
                StreamWriter stream = new StreamWriter(file);
                stream.WriteLine(OverAllResult);
                stream.Close();
                file.Close();
               
                OnDataUpdated();

            }
            else
            { //Штрафы отсутствуют
                resultwithbusts = $"{(int)ts_0.TotalSeconds:00}:{ts_0.Milliseconds:000}";
                string time = $"{(int)ts_0.TotalSeconds:00}:{ts_0.Milliseconds:000}";
                Result_plus_Busts.Text = resultwithbusts;
                round_number = Rounds.Text;
                team_name = Team_Name.Text;
                string teamname_result = round_number + team_name + resultwithbusts;
                FileStream file = new FileStream(path, FileMode.Append);
                StreamWriter stream = new StreamWriter(file);
                stream.WriteLine(teamname_result);
                stream.Close();
                file.Close();
                
                OnDataUpdated();
            }

        }

        private void Team_Name_Loaded(object sender, RoutedEventArgs e)
        {//Загрузка имен команд
            StreamReader reader = new StreamReader(path_teams);
            string x = reader.ReadToEnd();
            string[] y = x.Split('\n');
            foreach (string s in y)
            {
                Team_Name.Items.Add(s);
            }
        }

        void Grid_KeyDown(object sender, KeyEventArgs e)
        {//Управление с клавиатуры

            if (e.Key == Key.OemPlus)
            {
                Bust_Click(Bust, null);
            }
            if (e.Key == Key.OemMinus)
            {
                Bust_min_Click(Bust_min, null);
            }
        }

        private void Rounds_Loaded(object sender, RoutedEventArgs e) // Загрузка названий раундов
        {
            StreamReader reader_1 = new StreamReader(path_rounds);
            string x = reader_1.ReadToEnd();
            string[] y = x.Split('\n');
            foreach (string s in y)
            {
                Rounds.Items.Add(s);
            }
        }

        private void Stop_Round_Click(object sender, RoutedEventArgs e) // Кнопка окончания раунда
        {
            sp.Write("f");
            stopWatch.Stop();
            stop_timer.Set();
            string elapsedTime = String.Format("{0:00}:{1:000}", (int)ts_0.TotalSeconds, ts_0.Milliseconds);
            Dispatcher.Invoke(() => Result.Text = elapsedTime);
            stopWatch.Reset();
            off_q = 0;
            OnDataUpdated();

        }

        private void Print() // Сохранение данных срабатывания датчика
        {
            string result_each_tuch = ts_0.ToString("hh\\:mm\\:ss\\:fff");
            string result_each_tuch_1 = result_each_tuch;
            FileStream file = new FileStream(path_each_tuch, FileMode.Append);
            StreamWriter stream = new StreamWriter(file);
            stream.WriteLine(result_each_tuch_1);
            stream.Close();
            file.Close();
        }


        private void Lines_ON_Click(object sender, RoutedEventArgs e) // Включние линий синяя-зеленая
        {
            sp.Write("g");
        }

        private void Red_Signal_Click(object sender, RoutedEventArgs e) // Моргание линий красным цветом
        {
            sp.Write("r");
        }

        private void Open_Demo(object sender, RoutedEventArgs e) // Открытие демонстрационного окна
        {
            try
            {

                DemoWindow demo = new DemoWindow(this);
                demo.Show();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void OnDataUpdated() // Метод обновления данных
        {
            DataUpdated?.Invoke();
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

                Result.Text = elapsedTime; // Декоративное отображение
                OnDataUpdated(); // Уведомление в Demo окно
            }
        }
    }


}
