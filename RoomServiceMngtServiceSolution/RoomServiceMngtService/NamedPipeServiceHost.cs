using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using SharedContacts;

namespace RoomServiceMngtService
{
    public class NamedPipeServiceHost
    {
        public static ServiceHost ServiceHost;
        public static void StartServiceHost() {
             ServiceHost = new ServiceHost(typeof(Contract),
                         new Uri("net.pipe://localhost/desktop") );

            ServiceHost.AddServiceEndpoint(typeof(IContract),
              new NetNamedPipeBinding(),
              "desktop_client");

            ServiceDebugBehavior debug = ServiceHost.Description.Behaviors.Find<ServiceDebugBehavior>();

            // if not found - add behavior with setting turned on 
            if (debug == null)
            {
                ServiceHost.Description.Behaviors.Add(
                     new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            }
            else
            {
                // make sure setting is turned ON
                if (!debug.IncludeExceptionDetailInFaults)
                {
                    debug.IncludeExceptionDetailInFaults = true;
                }
            }
            ServiceHost.Open();
        }

        public static void StopServiceHost()
        {
            ServiceHost?.Close();
        }
        }
}
