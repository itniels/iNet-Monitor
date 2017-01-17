using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Web;
using System.Web.Http;
using iNetMonitor.Domain.ApiModels;
using iNetMonitor.Web.Models;

namespace iNetMonitor.Web.Controllers
{
    public class IpApiController : ApiController
    {
        public InternetInfoModel GetInfo()
        {
            InternetInfoModel model = new InternetInfoModel();
            // IP Address
            try
            {
                var ip = IPAddress.Parse(HttpContext.Current.Request.UserHostAddress);
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    model.IPv4 = ip.ToString();

                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                    model.IPv6 = HttpContext.Current.Request.UserHostAddress;

            }
            catch (Exception)
            {
                model.IPv4 = "Unknown!";
                model.IPv6 = "Unknown!";
            }

            // Hostname
            try
            {
                model.Hostname = Dns.GetHostEntry(HttpContext.Current.Request.UserHostName).HostName;
            }
            catch (Exception)
            {
                model.Hostname = "Unknown!";
            }


            // Tor node check
            try
            {
                string tip = HttpContext.Current.Request.UserHostAddress;

                string tor = tip + ".80." + tip.Reverse() + ".ip-port.exitlist.torproject.org";
                IPHostEntry torEntry = Dns.GetHostEntry(tor);
                if (torEntry.AddressList.Length > 0)
                    model.TorNode = torEntry.AddressList[0].ToString() == "127.0.0.2" ? "Yes" : "No";
                else
                    model.TorNode = "No";
            }
            catch (Exception)
            {
                model.TorNode = "No";
            }


            model.ISP = "TODO";
            model.Lok_Country = "TODO";
            model.Lok_City = "TODO";


            return model;
        }
    }
}