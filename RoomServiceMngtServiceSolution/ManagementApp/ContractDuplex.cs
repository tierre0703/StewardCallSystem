using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;
using SharedContacts;

namespace ManagementApp
{
    public class ContractDuplex : DuplexClientBase<IContract>
    {
        public ContractDuplex(object callbackInstance, Binding binding, EndpointAddress remoteAddress) : base(callbackInstance, binding, remoteAddress) { }
    }
}
