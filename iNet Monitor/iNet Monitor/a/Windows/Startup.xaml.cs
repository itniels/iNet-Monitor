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
using iNet_Monitor.a.Models;

namespace iNet_Monitor.a.Windows
{
    /// <summary>
    /// Interaction logic for Startup.xaml
    /// </summary>
    public partial class Startup : Window
    {
        public Startup()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
            // Initiate
            for (int i = 1; i <= 300; i++)
            {
                a.Assets.Data.PointsOnline.Add(new ChartPoint(i, -1));
            }
            //for (int i = 1; i <= 300; i++)
            //{
            //    a.Assets.Data.PointsOffline.Add(new ChartPoint(i, -1));
            //}

            var f = new a.Windows.Main();
            f.Owner = this;

            if (Properties.Settings.Default.StartMinimized)
            {
                f.Show();
                f.Hide();
            }
            else
            {
                f.Show();
            }
                
            
        }
    }
}
