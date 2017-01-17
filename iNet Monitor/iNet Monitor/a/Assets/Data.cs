using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iNet_Monitor.a.Models;

namespace iNet_Monitor.a.Assets
{
    public static class Data
    {
        public static ObservableCollection<ChartPoint> PointsOnline = new ObservableCollection<ChartPoint>();
        //public static ObservableCollection<ChartPoint> PointsSlow = new ObservableCollection<ChartPoint>();
        //public static ObservableCollection<ChartPoint> PointsOffline = new ObservableCollection<ChartPoint>();
        //public static ObservableCollection<ChartPoint> PointsUnknown = new ObservableCollection<ChartPoint>();
        public static string Hostname = "Searching...";
        public static string ComputerName = "Searching...";
        public static string LanIP = "Searching...";
        public static string WanIP = "Searching...";
        public static string DNS = "Searching...";
        public static eStatus Status = eStatus.Unknown;
        public static bool isStillRunningMessage = true;
        public static double counter = 300;
    }
}
