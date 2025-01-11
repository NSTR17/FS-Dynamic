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


namespace FS_Dynamic
{
    /// <summary>
    /// Логика взаимодействия для DemoWindow.xaml
    /// </summary>
    public partial class DemoWindow : Window
    {
        Stopwatch stopWatch = new Stopwatch();
        DateTime timer_1 = new DateTime(0, 0);
        DateTime timer_2 = new DateTime(0, 0);
        bool st_timer1 = false;
        AutoResetEvent stop_timer1 = new AutoResetEvent(false);
        int new_flag = 0;
        const int FlagOn = 2;
        Timer timer_db_on = new Timer(FlagOn);



        public DemoWindow()
        {
            InitializeComponent();

            



        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            try
            {
                ApplicationContext db = new ApplicationContext();
                List<Result> results = db.Results.ToList();

                listofResults.ItemsSource = results;

                List<Timer> timers = db.Timers.ToList();

                listofTimers.ItemsSource = timers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Reload()
        {
            try
            {

                ApplicationContext db = new ApplicationContext();

                //listofResults.ItemsSource = results;


                IQueryable<int> timers = db.Timers.Select(c => c.flag);
                List<int> flags = timers.ToList();
                new_flag = flags.Last();

                switch (new_flag)
                {
                    case 1:
                        ParameterizedThreadStart timer = new ParameterizedThreadStart(Timer);
                        Thread thread_1 = new Thread(Timer);
                        st_timer1 = true;
                        thread_1.Start((object)st_timer1);
                        db.Timers.Add(timer_db_on);
                        db.SaveChanges();
                        break;

                    case 0:
                        stop_timer1.Set();

                        break;

                }


                Thread.Sleep(1000);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void StartFunction(object sender, RoutedEventArgs e)
        {
            try
            {

                while (true)
                {
                    Reload();
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void Timer(object argument)
        {
            try
            {
                while (st_timer1)
                {
                    if (stop_timer1.WaitOne(0))
                    {
                        timer_1 = timer_2;
                        return;
                    }
                    stopWatch.Start();
                    TimeSpan rs = stopWatch.Elapsed;
                    string elapsedTime_Timer = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    rs.Hours, rs.Minutes, rs.Seconds, rs.Milliseconds);
                    try
                    {
                        Result_Demo.Text = elapsedTime_Timer;
                    }
                    catch (Exception ex) 
                    { 
                    MessageBox.Show(ex.Message);
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }






        }
    }
}
