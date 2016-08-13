using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iNet_Monitor.a.Models
{
    public class ChartPoint
    {
        public double x { get; set; }
        public double y { get; set; }

        public ChartPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
