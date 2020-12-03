using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedContacts
{
    public class MobileAppStatus
    {
        public string Employee { get; set; }
        public string Status { get; set; }
        public string Uptime { get; set; }

        [DisplayName("IP Address")]
        public string IpAddress { get; set; }      

        public int EmployeeId;

        public MobileAppStatus() {
        }

        public MobileAppStatus(string employee, string status, string uptime, string ipAddress)
        {
            Employee = employee;
            Status = status;
            IpAddress = ipAddress;
            Uptime = uptime;
        }
    }
}
