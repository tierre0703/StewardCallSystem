using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using SharedContacts;

namespace ManagementApp
{
    public class NamedPipeConnector
    {

        public static IContract Contract;
        public static bool IsConnected = false;
        public static bool ConnectToService()
        {
            try
            {
                var callback = new Callback();
                ContractDuplex client;
                client = new ContractDuplex(callback, new NetNamedPipeBinding() { Security = new NetNamedPipeSecurity() }, new EndpointAddress("net.pipe://localhost/desktop/desktop_client"));

                Contract = client.ChannelFactory.CreateChannel();
                Contract.GetMobileAppStatus();
                IsConnected = true;
                ((ICommunicationObject)Contract).Faulted += new EventHandler(ProxyServiceFactory_Faulted);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                IsConnected = false;
            }
            return IsConnected;
        }

        static void ProxyServiceFactory_Faulted(object sender, EventArgs e)

        {
            Console.WriteLine("Faulted *******************");
            IsConnected = false;

            ConnectToService();
        }
    }
}
