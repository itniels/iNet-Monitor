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

namespace iNet_Monitor.a.Windows
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load values
            chk_StartWithWindows.IsChecked = a.Logic.StartupManager.IsStartupCurrentUser();
            chk_CheckForUpdates.IsChecked = Properties.Settings.Default.CheckForUpdates;
            chk_ShowPopups.IsChecked = Properties.Settings.Default.ShowPopups;
            chk_WarnAboutSlow.IsChecked = Properties.Settings.Default.WarnSlow;
            chk_StartMinimized.IsChecked = Properties.Settings.Default.StartMinimized;

            txb_SlowThreshold.Text = Properties.Settings.Default.SlowThreshold.ToString();
            txb_IP.Text = Properties.Settings.Default.PingIP;

            int interval = Properties.Settings.Default.PingInterval;
            if (interval == 100)
                cmb_Interval.SelectedIndex = 0;
            if (interval == 200)
                cmb_Interval.SelectedIndex = 1;
            if (interval == 300)
                cmb_Interval.SelectedIndex = 2;
            if (interval == 500)
                cmb_Interval.SelectedIndex = 3;
            if (interval == 1000)
                cmb_Interval.SelectedIndex = 4;
            if (interval == 5000)
                cmb_Interval.SelectedIndex = 5;
            if (interval == 10000)
                cmb_Interval.SelectedIndex = 6;
            if (interval == 30000)
                cmb_Interval.SelectedIndex = 7;
            if (interval == 60000)
                cmb_Interval.SelectedIndex = 8;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Save_Click(object sender, RoutedEventArgs e)
        {
            bool valid = true;
            // Validate
            try
            {
                int value = Convert.ToInt32(txb_SlowThreshold.Text);
                if (txb_SlowThreshold.Text.Length == 0)
                    valid = false;
            }
            catch (Exception)
            {
                valid = false;
            }

            try
            {
                if (txb_IP.Text.Length == 0)
                    valid = false;
            }
            catch (Exception)
            {
                valid = false;
            }

            // Save values
            if (valid)
            {
                if (chk_StartWithWindows.IsChecked == true)
                    a.Logic.StartupManager.AddApplicationToCurrentUserStartup();
                else
                    a.Logic.StartupManager.RemoveApplicationFromCurrentUserStartup();

                Properties.Settings.Default.CheckForUpdates = chk_CheckForUpdates.IsChecked.Value;
                Properties.Settings.Default.ShowPopups = chk_ShowPopups.IsChecked.Value;
                Properties.Settings.Default.WarnSlow = chk_WarnAboutSlow.IsChecked.Value;
                Properties.Settings.Default.StartMinimized = chk_StartMinimized.IsChecked.Value;

                Properties.Settings.Default.SlowThreshold = Convert.ToInt32(txb_SlowThreshold.Text);
                Properties.Settings.Default.PingIP = txb_IP.Text;

                if (cmb_Interval.SelectedIndex == 0)
                    Properties.Settings.Default.PingInterval = 100;
                if (cmb_Interval.SelectedIndex == 1)
                    Properties.Settings.Default.PingInterval = 200;
                if (cmb_Interval.SelectedIndex == 2)
                    Properties.Settings.Default.PingInterval = 300;
                if (cmb_Interval.SelectedIndex == 3)
                    Properties.Settings.Default.PingInterval = 500;
                if (cmb_Interval.SelectedIndex == 4)
                    Properties.Settings.Default.PingInterval = 1000;
                if (cmb_Interval.SelectedIndex == 5)
                    Properties.Settings.Default.PingInterval = 5000;
                if (cmb_Interval.SelectedIndex == 6)
                    Properties.Settings.Default.PingInterval = 10000;
                if (cmb_Interval.SelectedIndex == 7)
                    Properties.Settings.Default.PingInterval = 30000;
                if (cmb_Interval.SelectedIndex == 8)
                    Properties.Settings.Default.PingInterval = 60000;

                Properties.Settings.Default.Save();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please make sure you have filled out everything correctly...", "Error!",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
