using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using iNet_Monitor.a.Assets;
using iNet_Monitor.a.Models;

namespace iNet_Monitor.a.Logic
{
    public static class Pinger
    {
        public static double GetPing()
        {
            double value = -1;
            try
            {
                Ping p = new Ping();
                PingReply reply = p.Send(Properties.Settings.Default.PingIP, 3000);
                if (reply != null)
                {
                    if (reply.Status != IPStatus.Success)
                    {
                        AddPing(-10);
                        AddPing(-10);
                        AddPing(-10);
                        a.Assets.Data.Status = eStatus.Offline;
                    }
                    else
                    {
                        AddPing(reply.RoundtripTime);
                        value = reply.RoundtripTime;
                    }
                }
                else
                {
                    AddPing(-10);
                    AddPing(-10);
                    AddPing(-10);
                    a.Assets.Data.Status = eStatus.Offline;
                }

            }
            catch (Exception)
            {
                AddPing(-10);
                AddPing(-10);
                AddPing(-10);
                a.Assets.Data.Status = eStatus.Offline;
            }

            if (value > 0 && value <= Properties.Settings.Default.SlowThreshold)
                a.Assets.Data.Status = eStatus.Online;
            if (value > Properties.Settings.Default.SlowThreshold)
                a.Assets.Data.Status = eStatus.Slow;

            return value;

        }

        private static void AddPing(long rtt)
        {
            double id = ++a.Assets.Data.counter;
            long value = rtt;
            double remVal = a.Assets.Data.counter - 300;

            a.Assets.Data.PointsOnline.RemoveAt(0);

            a.Assets.Data.PointsOnline.Add(new ChartPoint(id, value));

            //if (value > 0 && value <= Properties.Settings.Default.SlowThreshold)
            //    a.Assets.Data.PointsOnline.Add(new ChartPoint(id, value));
            //if (value > Properties.Settings.Default.SlowThreshold)
            //    a.Assets.Data.PointsOnline.Add(new ChartPoint(id, value));
            //if (value == -1)
            //    a.Assets.Data.PointsOnline.Add(new ChartPoint(id, value));

        }
    }
}
