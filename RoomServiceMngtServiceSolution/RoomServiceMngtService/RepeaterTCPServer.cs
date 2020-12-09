using RoomServiceMngtService.Factories;
using RoomServiceMngtService.Model;
using RoomServiceMngtService.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TCPServer;

namespace RoomServiceMngtService
{
    public class RepeaterTCPServer
    {

        public static RepeaterTCPServer Instance = new RepeaterTCPServer();

        public ConcurrentDictionary<string, TcpServerConnection> Connections = new ConcurrentDictionary<string, TcpServerConnection>();
        public ConcurrentDictionary<string, TimeSpan> ConnectionsTimespans = new ConcurrentDictionary<string, TimeSpan>();

        private TcpServer tcpServer;
        private System.Timers.Timer Timer;
        private System.Timers.Timer ConnectionEmailTimer;
        private ConcurrentDictionary<string, DateTime> lastEmailSentTimes = new ConcurrentDictionary<string, DateTime>();

        private RepeaterTCPServer() {
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

            Timer = new System.Timers.Timer(3000);
            Timer.Elapsed += OnTimedEvent;
            Timer.AutoReset = true;
            Timer.Start();

            ConnectionEmailTimer = new System.Timers.Timer(60000);
            ConnectionEmailTimer.Elapsed += OnTimedEvent2;
            ConnectionEmailTimer.AutoReset = true;
            ConnectionEmailTimer.Start();
        }

        private void OnTimedEvent2(object sender, ElapsedEventArgs e)
        {
            foreach (var item in lastEmailSentTimes)
            {
                DateTime time;
                string repeater = item.Key;
                bool t = lastEmailSentTimes.TryRemove(repeater, out time);
                if (t)
                {
                    Console.WriteLine("Repeater Connected - " + repeater + " at " + time.ToString("yyyy-MM-dd HH:mm:ss"));
                    EmailNotification.SendAsync("Repeater Connected", "A Repeater " + repeater + " Connected at " + time.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine
                            + DateTime.Now.ToString());
                }                                       
            }
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            foreach (var item in Connections)
            {
                try {
                    bool connected = false;
                    if (item.Value.verifyConnected())
                    {
                        try
                        {
                            NetworkStream stream = item.Value.m_socket.GetStream();
                            string data = "T";
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
                    Console.WriteLine("Repeater Disconnected - " + item.Key);
                    TcpServerConnection con;
                    bool r = Connections.TryRemove(item.Key, out con);
                        if (r)
                        {
                            EmailNotification.SendAsync("Repeater Disconnected", "A Repeater " + item.Key + " Disconnected." + Environment.NewLine
                                + DateTime.Now.ToString());

                            if (Contract.Callback != null)
                            {
                                TimeSpan timeSpan;
                                ConnectionsTimespans.TryGetValue(item.Key, out timeSpan);
                                Contract.Callback.NotifyRepeaterStatus(new SharedContacts.RepeaterStatus()
                                {
                                    AppId = item.Key,
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
                        //Console.WriteLine("Test");
                        TimeSpan timeSpan;
                        bool b = ConnectionsTimespans.TryGetValue(item.Key, out timeSpan);
                        if (b)
                        {
                           TimeSpan newT = timeSpan.Add(TimeSpan.FromMilliseconds(3000));
                           ConnectionsTimespans.TryUpdate(item.Key, newT, timeSpan);
                        }
                        
                        //ConnectionsTimespans[item.Key] = ConnectionsTimespans[item.Key].Add(TimeSpan.FromMilliseconds(3000));
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

        public void Initialize() {
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            Console.WriteLine("Initializing Repeater TCP Server on Port 9999");
            
            tcpServer.Close();
            tcpServer.Port = 9999;
            tcpServer.Open();

            Console.WriteLine("Repeater TCP Server Started");

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
                    Console.WriteLine("RECEIVED :: " + dataStr);
                    connection.sendData("ACK");

                    if (dataStr.Contains("H_SHAKE:"))
                    {
                        int index = dataStr.IndexOf("H_SHAKE:");
                        index = (index < 0) ? 0 : index;

                        string[] repeater = dataStr.Substring(index + "H_SHAKE:".Length, dataStr.Length - "H_SHAKE:".Length - 1).Split(':');
                        if (Connections.ContainsKey(repeater[0]))
                        {
                            TcpServerConnection con;
                            Connections.TryRemove(repeater[0], out con);
                        }
                        Connections.TryAdd(repeater[0], connection);
                        if (!ConnectionsTimespans.ContainsKey(repeater[0])) {
                            ConnectionsTimespans.TryAdd(repeater[0], TimeSpan.FromMilliseconds(0));
                        }
                        if (Contract.Callback != null)
                        {
                            Contract.Callback.NotifyRepeaterStatus(new SharedContacts.RepeaterStatus() {
                                AppId = repeater[0], Status = "Running" , IpAddress = connection.IpAddress,
                                Uptime = (ConnectionsTimespans[repeater[0]] == null) ?
                        "00:00:00:00" : ConnectionsTimespans[repeater[0]].ToString(@"dd\.hh\:mm\:ss")
                            });
                        }

                        if (lastEmailSentTimes.ContainsKey(repeater[0]))
                        {
                            DateTime lastEmailSentTime;
                            lastEmailSentTimes.TryRemove(repeater[0], out lastEmailSentTime);
                            lastEmailSentTimes.TryAdd(repeater[0], DateTime.Now);
                        }
                        else {
                            lastEmailSentTimes.TryAdd(repeater[0], DateTime.Now);
                        }
                    }
                    else if (dataStr.Contains("CALL:"))
                    {
                        int index = dataStr.IndexOf("CALL:");
                        index = (index < 0) ? 0 : index;
                        string[] roomId = dataStr.Substring(index + "CALL:".Length, dataStr.Length - "CALL:".Length - 1).Split(':');
                        Room room = RoomFactory.GetInstance().GetByUniqueId(roomId[0]);
                        if (room == null)
                        {
                            Console.WriteLine("room Null");
                            return;
                        }
                        string uniqueId = Guid.NewGuid().ToString();
                        CallService.ReceiveNewCall(new Call()
                        {
                            UniqueId = uniqueId,
                            Room = room,
                            Accepted = false,
                            TimeStamp = DateTime.Now
                        });
                    }
                    data = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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

        public void StopAll() {
            Timer.Stop();
            tcpServer.Close();
            Connections.Clear();
        }

        private void tcpServer_OnConnect(TcpServerConnection connection)
        {
            //invokeDelegate setText = () => lblConnected.Text = tcpServer1.Connections.Count.ToString();
           
            Console.WriteLine("Repeater :: No Of Connections : " + tcpServer.Connections.Count.ToString());

            //Invoke(setText);
        }

    }
}
