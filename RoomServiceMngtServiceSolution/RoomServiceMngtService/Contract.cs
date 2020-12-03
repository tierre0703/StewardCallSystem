using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using SharedContacts;
using TCPServer;
using RoomServiceMngtService.Factories;
using RoomServiceMngtService.Model;
using System.Collections.Concurrent;

namespace RoomServiceMngtService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Contract: IContract
    {
        public static ICallback Callback = null;
        public Contract()
        {
            Callback = OperationContext.Current.GetCallbackChannel<ICallback>();
            OperationContext.Current.Channel.Faulted += new EventHandler(Channel_Faulted);
            OperationContext.Current.Channel.Closed += new EventHandler(Channel_Closed);
        }

        void Channel_Faulted(object sender, EventArgs e)
        {
            Console.WriteLine("Contract :: Channel_Faulted() :: Channel Falted");
            Callback = null;
        }

        void Channel_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("Contract :: Channel_Closed() :: Channel Closed");
            Callback = null;
        }

        public IList<MobileAppStatus> GetMobileAppStatus()
        {
            ConcurrentDictionary<int, TcpServerConnection> Connections = MobileTCPServer.Instance.Connections;
            List<MobileAppStatus> list = new List<MobileAppStatus>();
            foreach (var item in Connections)
            {
                Employee e = EmployeeFactory.GetInstance().TryGet(item.Key);
                if (e != null) {
                    TimeSpan timespan;
                    MobileTCPServer.Instance.ConnectionsTimespans.TryGetValue(e.Id, out timespan);
                    list.Add(new MobileAppStatus() { Employee = e.Name,
                        Status = "Running", EmployeeId = e.Id,
                        IpAddress = item.Value.IpAddress,
                        Uptime = (timespan == null) ?
                        "00:00:00:00" : timespan.ToString(@"dd\.hh\:mm\:ss")
                    });
                }
            }

            return list;
        }

        public IList<RepeaterStatus> GetRepeaterStatus()
        {
            ConcurrentDictionary<string, TcpServerConnection> Connections = RepeaterTCPServer.Instance.Connections;
            List<RepeaterStatus> list = new List<RepeaterStatus>();
            foreach (var item in Connections)
            {
                TimeSpan timespan;
                RepeaterTCPServer.Instance.ConnectionsTimespans.TryGetValue(item.Key, out timespan);
                list.Add(new RepeaterStatus()
                {
                     AppId= item.Key,
                        Status = "Running",
                        IpAddress = item.Value.IpAddress,
                        Uptime = (timespan == null) ?
                        "00:00:00:00" : timespan.ToString(@"dd\.hh\:mm\:ss")
                });               
            }

            return list;
        }
    }
}
