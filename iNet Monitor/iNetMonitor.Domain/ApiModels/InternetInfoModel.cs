using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iNetMonitor.Domain.ApiModels
{
    public class InternetInfoModel
    {
        public string IPv4 { get; set; }
        public string IPv6 { get; set; }
        public string Hostname { get; set; }
        public string Lok_Country { get; set; }
        public string Lok_City { get; set; }
        public string ISP { get; set; }
        public string TorNode { get; set; }
    }
}