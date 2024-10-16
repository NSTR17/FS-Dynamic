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

namespace FS_Dynamic
{
    /// <summary>
    /// Логика взаимодействия для DemoWindow.xaml
    /// </summary>
    public partial class DemoWindow : Window
    {
        int k = 1;

        public DemoWindow()
        {
            InitializeComponent();

            try
            {
                do
                {
                    Timer();
                }
                while (k != 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            //Team_Name.Text = Data.Choosen_TeamName; 

            /* try
             {

                do
                 {
                     ApplicationContext applicationContext = new ApplicationContext();
                     List<Result> results = applicationContext.Results.ToList();

                     listofResults.ItemsSource = results;

                     List<Timer> timers = applicationContext.Timers.ToList();

                     listofTimers.ItemsSource = timers;

                     //Thread.Sleep(10000);
                 }
                while (k != 0);


             }

             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);

             }

             */



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

        private void Timer()
        {
            try
            {
               
                    ApplicationContext applicationContext = new ApplicationContext();
                    List<Result> results = applicationContext.Results.ToList();

                    listofResults.ItemsSource = results;

                    List<Timer> timers = applicationContext.Timers.ToList();

                    listofTimers.ItemsSource = timers;

                    Thread.Sleep(100000);
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

    }
}
