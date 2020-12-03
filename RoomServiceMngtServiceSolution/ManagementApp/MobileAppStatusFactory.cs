using SharedContacts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementApp
{
    public class MobileAppStatusFactory
    {
        public static MobileAppStatusFactory Instance = new MobileAppStatusFactory();

        public BindingList<MobileAppStatus> StatusList = new BindingList<MobileAppStatus>();
        private MobileAppStatusFactory() {
        }

        public MobileAppStatus Get(int employeeId) {
            foreach (var item in StatusList)
            {
                if (item.EmployeeId == employeeId) {
                    return item;
                }
            }
            return null;
        }

        public void InitList(IList<MobileAppStatus> list) {
            StatusList.Clear();
            foreach (var item in list)
            {
                StatusList.Add(item);
            }
        }

        public void UpdateWaiting() {
            foreach (var item in StatusList)
            {
                item.Status = "Waiting";
            }
            }

        public void UpdateList(IList<MobileAppStatus> list) {
            //StatusList.Clear();
            foreach (var item in StatusList)
            {
                foreach (var i in list)
                {
                    if (item.EmployeeId == i.EmployeeId)
                    {
                        item.Status = i.Status;
                        item.IpAddress = i.IpAddress;
                        item.Uptime = i.Uptime;
                        break;
                    }
                }
            }         
        }
    }
}
