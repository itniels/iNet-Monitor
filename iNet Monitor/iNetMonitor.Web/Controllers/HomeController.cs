using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using iNetMonitor.Domain.ApiModels;
using iNetMonitor.Web.Models;

namespace iNetMonitor.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            InternetInfoModel model = new InternetInfoModel();

            // IP Address
            try
            {
                var ip = IPAddress.Parse(Request.UserHostAddress);
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    model.IPv4 = ip.ToString();

                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                    model.IPv6 = Request.UserHostAddress;

            }
            catch (Exception)
            {
                model.IPv4 = "Sorry could not determine you ip :-(";
                model.IPv6 = "Sorry could not determine you ip :-(";
            }

            // Hostname
            try
            {
                model.Hostname = Dns.GetHostEntry(Request.UserHostName).HostName;
            }
            catch (Exception)
            {
                model.Hostname = "Sorry could not determine you hostname! :-(";
            }


            // Tor node check
            try
            {
                string tor = helper_ReverseIp(Request.UserHostAddress) + ".443." + helper_ReverseIp("148.251.244.75") + ".ip-port.exitlist.torproject.org";
                IPHostEntry torEntry = Dns.GetHostEntry(tor);
                model.TorNode = torEntry.AddressList.Length > 0 ? "Yes" : "No";
            }
            catch (Exception)
            {
                model.TorNode = "No";
            }


            model.ISP = "TODO";
            model.Lok_Country = "TODO";
            model.Lok_City = "TODO";


            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private string helper_ReverseIp(string ip)
        {
            string result = "";
            List<string> octets = ip.Split('.').ToList();
            octets.Reverse();
            bool isFirst = true;
            foreach (string octet in octets)
            {
                if (isFirst)
                    result += octet;
                else
                    result += "." + octet;

                isFirst = false;
            }
            return result;
        }
    }
}