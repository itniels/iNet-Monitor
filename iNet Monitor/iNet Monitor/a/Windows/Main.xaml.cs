using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using iNet_Monitor.a.Assets;
using Sparrow.Chart;
using Application = System.Windows.Application;
using ChartPoint = iNet_Monitor.a.Models.ChartPoint;
using Clipboard = System.Windows.Forms.Clipboard;

namespace iNet_Monitor.a.Windows
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        BackgroundWorker bg_worker = new BackgroundWorker();
        private bool isFirstBGRun = true;
        private bool isShuttingDown = false;
        private eStatus lastStatus = a.Assets.Data.Status;
        public Main()
        {
            InitializeComponent();
            // EventHandlers
            timer.Tick += timer_tick;
            bg_worker.DoWork += bg_worker_DoWork;
            bg_worker.RunWorkerCompleted += bg_worker_Completed;
            MyNotifyIcon.TrayMouseDoubleClick += new RoutedEventHandler(OnNotifyIcon_Click);
        }

        private void OnNotifyIcon_Click(object sender, RoutedEventArgs e)
        {
            this.Show();
            PlaceLowerRight();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PlaceLowerRight();
            bg_worker.RunWorkerAsync();
            PingChart.DataContext = new a.ViewModels.ChartViewModel();



            timer.Interval = new TimeSpan(0, 0, 0, 0, Properties.Settings.Default.PingInterval);
            timer.Start();
        }

        private void PlaceLowerRight()
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        private void bg_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!isFirstBGRun)
            {
                // Sleep for 30 seconds before attempting again!
                Thread.Sleep(30000);
            }
            a.Logic.GetSystemInfo.GetComputerName();
            a.Logic.GetSystemInfo.GetLanIP();
            a.Logic.GetSystemInfo.GetWanIP();
            a.Logic.GetSystemInfo.GetHostname();
        }

        private void bg_worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (a.Assets.Data.Hostname != "Searching..." && a.Assets.Data.Hostname != "Waiting for WAN IP...")
            {
                isFirstBGRun = false;
            }

            bg_worker.RunWorkerAsync();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            // Set interval
            //timer.Interval = new TimeSpan(0, 0, 0, 0, Properties.Settings.Default.PingInterval);

            // Get values
            double value = a.Logic.Pinger.GetPing();

            // Calculate Avg
            try
            {
                List<double> values = new List<double>();
                foreach (ChartPoint point in a.Assets.Data.PointsOnline)
                {
                    if (point.y > -1)
                    {
                        values.Add(point.y);
                    }
                }

                double average = values.Average();
                lbl_AvgLatency.Content = average.ToString("0.0") + " ms";
                lbl_MinMax.Content = values.Min().ToString("0.0") + " / " + values.Max().ToString("0.0") + " ms";
            }
            catch (Exception)
            {
                lbl_AvgLatency.Content = "No data...";
                lbl_MinMax.Content = "No data...";
            }


            // Refresh UI
            lbl_Hostname.Content = a.Assets.Data.Hostname;
            lbl_LanIP.Content = a.Assets.Data.LanIP;
            lbl_WanIP.Content = a.Assets.Data.WanIP;
            lbl_ComputerName.Content = a.Assets.Data.ComputerName;

            if (value >= 0)
            {
                lbl_Latency.Content = value + " ms";
            }
            else
            {
                lbl_Latency.Content = "No data...";
            }
            lbl_Status.Content = a.Assets.Data.Status.ToString().ToUpper();

            // Color
            if (a.Assets.Data.Status == eStatus.Unknown)
                lbl_Status.Foreground = new SolidColorBrush(Colors.Gray);
            if (a.Assets.Data.Status == eStatus.Offline)
                lbl_Status.Foreground = new SolidColorBrush(Colors.Red);
            if (a.Assets.Data.Status == eStatus.Slow)
                lbl_Status.Foreground = new SolidColorBrush(Colors.Yellow);
            if (a.Assets.Data.Status == eStatus.Online)
                lbl_Status.Foreground = new SolidColorBrush(Colors.GreenYellow);



            // NotifyIcon
            SetIcon();
            //Notifications
            ShowNotification();
        }

        private void SetIcon()
        {
            string unknown = @"pack://application:,,/Resources/Unknown.ico";
            string offline = @"pack://application:,,/Resources/Offline.ico";
            string slow = @"pack://application:,,/Resources/Slow.ico";
            string online = @"pack://application:,,/Resources/Online.ico";

            if (a.Assets.Data.Status == eStatus.Unknown)
            {
                Stream iconStream = Application.GetResourceStream(new Uri(unknown, UriKind.RelativeOrAbsolute)).Stream;
                MyNotifyIcon.Icon = new System.Drawing.Icon(iconStream);
                MyNotifyIcon.ToolTipText = "iNet Monitor (UNKNOWN!)";
            }
            if (a.Assets.Data.Status == eStatus.Offline)
            {
                Stream iconStream = Application.GetResourceStream(new Uri(offline, UriKind.RelativeOrAbsolute)).Stream;
                MyNotifyIcon.Icon = new System.Drawing.Icon(iconStream);
                MyNotifyIcon.ToolTipText = "iNet Monitor (OFFLINE!)";
            }
            if (a.Assets.Data.Status == eStatus.Slow)
            {
                Stream iconStream = Application.GetResourceStream(new Uri(slow, UriKind.RelativeOrAbsolute)).Stream;
                MyNotifyIcon.Icon = new System.Drawing.Icon(iconStream);
                MyNotifyIcon.ToolTipText = "iNet Monitor (SLOW!)";
            }
            if (a.Assets.Data.Status == eStatus.Online)
            {
                Stream iconStream = Application.GetResourceStream(new Uri(online, UriKind.RelativeOrAbsolute)).Stream;
                MyNotifyIcon.Icon = new System.Drawing.Icon(iconStream);
                MyNotifyIcon.ToolTipText = "iNet Monitor (ONLINE!)";
            }
        }

        private void ShowNotification()
        {
            if (Properties.Settings.Default.ShowPopups)
            {
                if (lastStatus != a.Assets.Data.Status)
                {
                    //LinePartBase lpb = new LinePartBase();

                    //if (a.Assets.Data.Status == eStatus.Online)
                    //    lpb.Stroke = new SolidColorBrush(Colors.GreenYellow);
                    //if (a.Assets.Data.Status == eStatus.Slow)
                    //    lpb.Stroke = new SolidColorBrush(Colors.Yellow);
                    //if (a.Assets.Data.Status == eStatus.Offline)
                    //    lpb.Stroke = new SolidColorBrush(Colors.Red);

                    //LineSeries.Parts.Add(lpb);
                    

                    if (a.Assets.Data.Status == eStatus.Slow && !Properties.Settings.Default.WarnSlow)
                    {
                        // Do Nothing!
                    }
                    else
                    {
                        lastStatus = a.Assets.Data.Status;
                        MyNotifyIcon.ShowBalloonTip("iNet Monitor",
                            "Connection is now " + lastStatus, BalloonIcon.Info);
                    }

                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!isShuttingDown)
            {
                e.Cancel = true;
                this.Hide();
                if (Properties.Settings.Default.IsFirstTimeMinimize)
                {
                    MyNotifyIcon.ShowBalloonTip("iNet Monitor",
                        "Im still running here in your tray, doubleclick to get me back :-)", BalloonIcon.Info);
                    Properties.Settings.Default.IsFirstTimeMinimize = false;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void Img_Copy_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Image img = (Image)sender;
            img.Opacity = 1.0;
        }

        private void Img_Copy_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Image img = (Image)sender;
            img.Opacity = 0.5;
        }

        private void Img_Copy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            img.Width = 12;
        }

        private void Img_Copy_ComputerName_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            img.Width = 16;
            Clipboard.SetText(a.Assets.Data.ComputerName);
            MyNotifyIcon.ShowBalloonTip("iNet Monitor", "Computer Name was copied to clipboard.", BalloonIcon.Info);
        }

        private void Img_Copy_Hostname_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            img.Width = 16;
            Clipboard.SetText(a.Assets.Data.Hostname);
            MyNotifyIcon.ShowBalloonTip("iNet Monitor", "Hostname was copied to clipboard.", BalloonIcon.Info);
        }

        private void Img_Copy_LanIP_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            img.Width = 16;
            Clipboard.SetText(a.Assets.Data.LanIP);
            MyNotifyIcon.ShowBalloonTip("iNet Monitor", "LAN IP was copied to clipboard.", BalloonIcon.Info);
        }

        private void Img_Copy_WanIP_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            img.Width = 16;
            Clipboard.SetText(a.Assets.Data.WanIP);
            MyNotifyIcon.ShowBalloonTip("iNet Monitor", "WAN IP was copied to clipboard.", BalloonIcon.Info);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Exit
            isShuttingDown = true;
            Application.Current.Shutdown();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            // Settings
            var f = new a.Windows.Settings
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            f.ShowDialog();
            timer.Interval = new TimeSpan(0, 0, 0, 0, Properties.Settings.Default.PingInterval);
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var f = new a.Windows.About();
            f.Owner = this;
            f.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            f.ShowDialog();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=HARWVNKCQWQYW");
        }
    }


}
