using SharedContacts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementApp
{
    public class RepeaterStatusFactory
    {
        public static RepeaterStatusFactory Instance = new RepeaterStatusFactory();

        public BindingList<RepeaterStatus> StatusList = new BindingList<RepeaterStatus>();
        private RepeaterStatusFactory()
        {
        }

        //public void InitList(IList<RepeaterStatus> list)
        //{
        //    StatusList.Clear();
        //    foreach (var item in list)
        //    {
        //        StatusList.Add(item);
        //    }
        //}

        public void UpdateWaiting()
        {
            foreach (var item in StatusList)
            {
                item.Status = "Waiting";
            }
        }

        public RepeaterStatus Get(string appId)
        {
            foreach (var item in StatusList)
            {
                if (item.AppId.CompareTo(appId) == 0)
                {
                    return item;
                }
            }
            return null;
        }

        public void UpdateList(IList<RepeaterStatus> list)
        {
            foreach (var item in list)
            {
                bool found = false;
                foreach (var i in StatusList)
                {
                    if (item.AppId.CompareTo(i.AppId) == 0)
                    {
                        i.Status = item.Status;
                        i.IpAddress = item.IpAddress;
                        i.Uptime = item.Uptime;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    StatusList.Add(item);
                }
            }
        }
    }
}
