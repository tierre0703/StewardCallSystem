using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SharedContacts
{
    [ServiceContract]
    public interface ICallback
    {
        [OperationContract]
        void NotifyMobileAppStatus(int employeeId, MobileAppStatus status);

        [OperationContract]
        void NotifyRepeaterStatus(RepeaterStatus status);
    }
}
