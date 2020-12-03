using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace SharedContacts
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ICallback))]
    public interface IContract
    {
        [OperationContract]
        IList<MobileAppStatus> GetMobileAppStatus();

        [OperationContract]
        IList<RepeaterStatus> GetRepeaterStatus();
    }
}
