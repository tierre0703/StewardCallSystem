using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedContacts
{
    public class RepeaterStatus
    {
        public string AppId { get; set; }
        public string Status { get; set; }    

        public string Uptime { get; set; }

        [DisplayName("IP Address")]
        public string IpAddress { get; set; }

        public RepeaterStatus()
        {
        }

        public RepeaterStatus(string appId, string status, string uptime, string ipAddress)
        {
            AppId = appId;
            Status = status;
            IpAddress = ipAddress;
            Uptime = uptime;
        }
    }
}
