using SharedContacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementApp
{
    public class Callback : ICallback
    {
        public void NotifyMobileAppStatus(int employeeId, MobileAppStatus status)
        {
            MobileAppStatus old = MobileAppStatusFactory.Instance.Get(employeeId);
            if (old != null)
            {
                old.Status = status.Status;
            }
            else {
                MobileAppStatusFactory.Instance.StatusList.Add(status);
            }
        }

        public void NotifyRepeaterStatus(RepeaterStatus status)
        {
            RepeaterStatus old = RepeaterStatusFactory.Instance.Get(status.AppId);
            if (old != null)
            {
                old.Status = status.Status;
            }
            else
            {
                RepeaterStatusFactory.Instance.StatusList.Add(status);
            }
        }
    }
}
