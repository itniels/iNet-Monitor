using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iNet_Monitor.a.Models;

namespace iNet_Monitor.a.ViewModels
{
    public class ChartViewModel
    {
        public ObservableCollection<ChartPoint> Online { get; set; }
        //public ObservableCollection<ChartPoint> Slow { get; set; }
        //public ObservableCollection<ChartPoint> Offline { get; set; }
        //public ObservableCollection<ChartPoint> Unknown { get; set; }
        public ChartViewModel()
        {
            Online = a.Assets.Data.PointsOnline;
            //Slow = a.Assets.Data.PointsSlow;
            //Offline = a.Assets.Data.PointsOffline;
            //Unknown = a.Assets.Data.PointsUnknown;
        }
        

        
    }
}
