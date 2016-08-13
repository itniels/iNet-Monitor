using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace iNet_Monitor.a.Logic
{
    public static class GetSystemInfo
    {
        public static void GetHostname()
        {
            string hostname = "Unable to obtain!";
            if (a.Assets.Data.WanIP != "Searching...")
            {
                try
                {
                    IPAddress addr = IPAddress.Parse(a.Assets.Data.WanIP);
                    IPHostEntry entry = Dns.GetHostEntry(addr);
                    hostname = entry.HostName;
                }
                catch (Exception)
                {
                    hostname = "Unable to obtain!";
                }
            }
            else
            {
                hostname = "Waiting for WAN IP...";
            }

            // Set
            if (hostname == "Unable to obtain!")
            {
                if (a.Assets.Data.Hostname == "Searching...")
                    a.Assets.Data.Hostname = hostname;
            }
            else
            {
                a.Assets.Data.Hostname = hostname;
            }

            
        }

        public static void GetComputerName()
        {
            string computerName = "Unable to obtain!";
            try
            {
                computerName = System.Net.Dns.GetHostName();
            }
            catch (Exception)
            {
                computerName = "Unable to obtain!";
            }

            a.Assets.Data.ComputerName = computerName;
        }

        public static void GetLanIP()
        {
            string IP = "Unable to obtain!";
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        IP = ip.ToString();
                    }
                }
            }
            catch (Exception)
            {
                IP = "Unable to obtain!";
            }

            a.Assets.Data.LanIP = IP;
        }

        public static void GetWanIP()
        {
            string direction = "Unable to obtain!";
            try
            {
                WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                using (WebResponse response = request.GetResponse())
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    direction = stream.ReadToEnd();
                }

                //Search for the ip in the html
                int first = direction.IndexOf("Address: ") + 9;
                int last = direction.LastIndexOf("</body>");
                direction = direction.Substring(first, last - first);

                // If we get any HTM tags...
                if (direction.Contains("<"))
                    direction = "Unable to obtain!";

            }
            catch (Exception)
            {
                direction = "Unable to obtain!";
            }

            // Set
            if (direction == "Unable to obtain!")
            {
                if (a.Assets.Data.WanIP == "Searching...")
                    a.Assets.Data.WanIP = direction;
            }
            else
            {
                a.Assets.Data.WanIP = direction;
            }
            
        }
    }
}
