using FS_Dynamic.Models;
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

        public OperatorWindow()
        {
            InitializeComponent();

            InitializeDecorativeTimer();
            COM.ItemsSource = ports;
            sp.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
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


        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                stopWatch.Stop();
                string elapsedTime = String.Format("{0:00}:{1:000}", (int)ts_0.TotalSeconds, ts_0.Milliseconds);
                Dispatcher.Invoke(() => txtPrevTime.Text = elapsedTime);
                Dispatcher.Invoke(() => txtStopWatch.Text = "00:000");
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
    }
}
