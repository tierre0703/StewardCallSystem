using RoomServiceMngtService.Factories;
using RoomServiceMngtService.Model;
using RoomServiceMngtService.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TCPServer;
using System.Threading;
using System.Collections.Concurrent;

namespace RoomServiceMngtService
{
    public class MobileTCPServer
    {
        public static MobileTCPServer Instance = new MobileTCPServer();

        public ConcurrentDictionary<int, TcpServerConnection> Connections = new ConcurrentDictionary<int, TcpServerConnection>();
        public ConcurrentDictionary<int, TimeSpan> ConnectionsTimespans = new ConcurrentDictionary<int, TimeSpan>();

        private System.Timers.Timer Timer;
        private System.Timers.Timer Timer2;

        public Queue<Message> MessageQueue = new Queue<Message>();

        public class Message
        {
            public int EmployeeId;
            public string Msg;
        }

        private TcpServer tcpServer;

        private MobileTCPServer()
        {
            tcpServer = new TcpServer();
            this.tcpServer.IdleTime = 50;
            this.tcpServer.IsOpen = false;
            this.tcpServer.MaxCallbackThreads = 100;
            this.tcpServer.MaxSendAttempts = 3;
            this.tcpServer.Port = -1;
            this.tcpServer.VerifyConnectionInterval = 0;
            this.tcpServer.OnConnect += new tcpServerConnectionChanged(this.tcpServer_OnConnect);
            this.tcpServer.OnDataAvailable += new tcpServerConnectionChanged(this.tcpServer_OnDataAvailable);
            this.tcpServer.OnError += new tcpServerError(this.tcpServer_OnError);

            Timer = new System.Timers.Timer(10000);
            Timer.Elapsed += OnTimedEvent;
            Timer.AutoReset = true;
            Timer.Start();

            Timer2 = new System.Timers.Timer(1000);
            Timer2.Elapsed += OnTimedEvent2;
            Timer2.AutoReset = true;
            Timer2.Start();
        }

        private void OnTimedEvent2(object sender, ElapsedEventArgs e)
        {
            while (true)
            {
                try
                {              
                if (MessageQueue.Count == 0)
                    return;

                Message message = MessageQueue.Peek();
                if (Connections.ContainsKey(message.EmployeeId))
                {
                    Send(Connections[message.EmployeeId], message.Msg);
                    MessageQueue.Dequeue();
                }
                else
                {
     
                }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }           
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            foreach (var item in Connections)
            {
                try
                {
                    Employee emp = EmployeeFactory.GetInstance().TryGet(item.Key);
                    bool connected = false;
                    if (item.Value.verifyConnected())
                    {
                            try
                            {
                                NetworkStream stream = item.Value.m_socket.GetStream();
                                string data = "PING:";
                                stream.Write(item.Value.m_encoding.GetBytes(data), 0, data.Length);
                                stream.Flush();

                                connected = true;
                            }
                            catch (Exception ex)
                            {
                                connected = false;
                            }
                    }
                    else
                    {
                        connected = false;
                    }
                    if (!connected)
                    {
                        Console.WriteLine("Mobile :: " + emp.Name + " Off-line");
                        TcpServerConnection con;
                        Connections.TryRemove(item.Key, out con);
                        if (Contract.Callback != null)
                        {
                            if (emp != null)
                            {
                                TimeSpan timeSpan;
                                ConnectionsTimespans.TryGetValue(item.Key, out timeSpan);
                                Contract.Callback.NotifyMobileAppStatus(emp.Id, new SharedContacts.MobileAppStatus()
                                {
                                    Employee = emp.Name,
                                    Status = "Off-line",
                                    IpAddress = item.Value.IpAddress,
                                    Uptime = (timeSpan == null) ?
                            "00:00:00:00" : timeSpan.ToString(@"dd\.hh\:mm\:ss")
                                });
                            }
                        }
                    }
                    else
                    {
                        TimeSpan timeSpan;
                        bool b = ConnectionsTimespans.TryGetValue(item.Key, out timeSpan);
                        if (b)
                        {
                            TimeSpan newT = timeSpan.Add(TimeSpan.FromMilliseconds(10000));
                            ConnectionsTimespans.TryUpdate(item.Key, newT, timeSpan);
                        }
                        //ConnectionsTimespans[emp.Id] = ConnectionsTimespans[emp.Id].Add(TimeSpan.FromMilliseconds(3000));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
        private void tcpServer_OnError(TcpServer server, Exception e)
        {
            
        }

        public void Send(TcpServerConnection connection, string data)
        {
            tcpServer.Send(connection, data);
        }

        public void SendToAll(string data)
        {
            foreach (var item in tcpServer.Connections)
            {
                tcpServer.Send(item, data);
            }         
        }

        public void Initialize()
        {
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            Console.WriteLine("Initializing Mobile TCP Server on Port 7777");

            tcpServer.Close();
            tcpServer.Port = 7777;
            tcpServer.Open();

            Console.WriteLine("Mobile TCP Server Started");
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");

        }

        private void tcpServer_OnDataAvailable(TcpServerConnection connection)
        {
            try
            {
                byte[] data = readStream(connection.Socket);
                if (data != null)
                {
                    string dataStr = Encoding.ASCII.GetString(data);
                    //Console.WriteLine("RECEIVED :: " + dataStr);
                    if (dataStr.Contains("BBCB_NC"))
                    {
                        Console.WriteLine("New Connection Request");
                        connection.sendData(ConnectionStrings.CONNECTION_ESTABLISHED);
                    }
                    else if (dataStr.Contains("AUTH:"))
                    {
                        Console.WriteLine("New Authentication Request");
                        int index = dataStr.IndexOf("AUTH:");
                        index = (index < 0) ? 0 : index;
                        string sub = dataStr.Substring(index + "AUTH:".Length);
                        string[] colonSplit = sub.Split(':');
                        string[] arr = colonSplit[0].Split(new char[0]);
                        string username = null;
                        string password = null;
                        foreach (var item in arr)
                        {
                            if (item.StartsWith("U-"))
                            {
                                username = item.Substring("U-".Length);
                            }
                            else if (item.StartsWith("P-"))
                            {
                                password = item.Substring("P-".Length);
                            }
                        }
                        if (username != null && password != null)
                        {
                            Employee employee;
                            Console.WriteLine("Username-" + username + " Password-" + password);
                            bool b = EmployeeService.CheckEmplyeeAuth(username, password, out employee);
                            //Connections.ContainsKey(employee.Id);
                            if (b)
                            {
                                Console.WriteLine("Authentication Successful :: Employee Name-" + employee.Name);
                                connection.sendData(ConnectionStrings.AUTHENTICATED);
                                connection.sendData(ConnectionStrings.EMP_DETAILS + employee.Id + ":" + employee.Name + ":");
                                if (Connections.ContainsKey(employee.Id))
                                {
                                    TcpServerConnection con;
                                    Connections.TryRemove(employee.Id, out con);
                                    con.forceDisconnect();
                                }
                                Connections.TryAdd(employee.Id, connection);
                                if (!ConnectionsTimespans.ContainsKey(employee.Id))
                                {
                                    ConnectionsTimespans.TryAdd(employee.Id, TimeSpan.FromMilliseconds(0));
                                }
                                if (Contract.Callback != null)
                                {
                                    Employee emp = EmployeeFactory.GetInstance().TryGet(employee.Id);
                                    if (emp != null)
                                    {
                                        Contract.Callback.NotifyMobileAppStatus(employee.Id, new SharedContacts.MobileAppStatus()
                                        {
                                            Employee = emp.Name,
                                            Status = "Running",
                                            EmployeeId = emp.Id,
                                            IpAddress = connection.IpAddress,
                                            Uptime = (ConnectionsTimespans[emp.Id] == null) ?
                        "00:00:00:00" : ConnectionsTimespans[emp.Id].ToString(@"dd\.hh\:mm\:ss")
                                        });
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Authentication Failed");
                                connection.sendData(ConnectionStrings.NOT_AUTHENTICATED);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Authentication Failed");
                            connection.sendData(ConnectionStrings.NOT_AUTHENTICATED);
                        }


                    }
                    else if (dataStr.Contains("ACCEPT:"))
                    {
                        int index = dataStr.IndexOf("ACCEPT:");
                        index = (index < 0) ? 0 : index;
                        string payload = dataStr.Substring(index + "ACCEPT:".Length);
                        if (payload != null)
                        {

                            string[] arr = payload.Split(':');
                            int id = int.Parse(arr[1]);
                            Console.WriteLine("Accept Call :: Employee ID-" + id + " :: " + arr[0]);
                            CallService.ReceiveCallAcceptRespond(id, arr[0]);
                        }
                    }
                    else if (dataStr.Contains("CANCEL:"))
                    {
                        int index = dataStr.IndexOf("CANCEL:");
                        index = (index < 0) ? 0 : index;
                        string payload = dataStr.Substring(index + "CANCEL:".Length);
                        if (payload != null)
                        {
                            string[] arr = payload.Split(':');
                            int id = int.Parse(arr[1]);
                            Console.WriteLine("Cancel Call :: Employee ID-" + id + " :: " + arr[0]);
                            CallService.ReceiveCallCancelRespond(id, arr[0]);
                        }
                    }
                    else if (dataStr.Contains("MPING"))
                    {
                        Console.WriteLine("Ping..   " + connection.IpAddress);
                    }
                    
                    data = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                try
                {
                    connection.sendData(ConnectionStrings.ERROR);
                }
                catch (Exception xx)
                {
                    Console.WriteLine(xx.Message);
                }               
            }
        }

        public void StopAll()
        {
            Timer.Stop();
            tcpServer.Close();
            Connections.Clear();

            SendToAll(ConnectionStrings.DISCONNECT);
        }

        protected byte[] readStream(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            if (stream.DataAvailable)
            {
                byte[] data = new byte[client.Available];

                int bytesRead = 0;
                try
                {
                    bytesRead = stream.Read(data, 0, data.Length);
                }
                catch (IOException)
                {
                }

                if (bytesRead < data.Length)
                {
                    byte[] lastData = data;
                    data = new byte[bytesRead];
                    Array.ConstrainedCopy(lastData, 0, data, 0, bytesRead);
                }
                return data;
            }
            return null;
        }

        private void tcpServer_OnConnect(TcpServerConnection connection)
        {
            //invokeDelegate setText = () => lblConnected.Text = tcpServer1.Connections.Count.ToString();

            Console.WriteLine("Mobile :: No Of Connections : " + tcpServer.Connections.Count.ToString());

            //Invoke(setText);
        }
    }
}
