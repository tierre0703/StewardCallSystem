using RoomServiceMngtService.DataAccess;
using RoomServiceMngtService.Factories;
using RoomServiceMngtService.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TCPServer;

namespace RoomServiceMngtService.Services
{
    public class CallService
    {
        private static object thisLock = new object();
        //private static Timer Timer;
        private static Timer Timer2;
        

        public static void LoadRecentUnacceptedCalls() {
            DateTime fromDate = DateTime.Now;
            fromDate = fromDate.AddMinutes(-10);

            DateTime toDate = DateTime.Now;
            List<Call> calls = CallData.GetInstance().GetRecentUnacceptedCalls(fromDate, toDate);
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            Console.WriteLine("No of Unaccepted Calls - Last 10 Minutes - " + calls.Count);
            foreach (var call in calls)
            {              
                CallFactory.GetInstance().Add(call.UniqueId, call);
                Console.WriteLine(call.TimeStamp.ToString() +" "+call.Room+ " " + call.UniqueId);
            }
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");

        }

        //public static void StartCallDatabaseService()
        //{
        //    Timer = new Timer(3000);
        //    Timer.Elapsed += OnTimedEvent;
        //    Timer.AutoReset = true;
        //    Timer.Start();
        //}

        public static void StartUnacceptedCallCheck()
        {
            Timer2 = new Timer(2000);
            //Timer2 = new Timer(5000);
            Timer2.Elapsed += OnTimedEvent2;
            Timer2.AutoReset = true;
            Timer2.Start();
        }

        //public static void StopCallDatabaseService()
        //{
        //    Timer?.Stop();
        //}

        public static void StopUnacceptedCallCheck()
        {
            Timer2?.Stop();
        }

        private static void OnTimedEvent2(object sender, ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
            foreach (var item in CallFactory.GetInstance().GetAll())
            {
                try { 
                if (!item.Accepted && now.Subtract(item.TimeStamp).TotalMilliseconds >= 30000) {
                        Console.WriteLine("RECALL :: " + item.Room.Number +" "+item.UniqueId);
                    Recall(item);
                }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        //{
        //    ConcurrentQueue<Call> queue = CallFactory.GetInstance().CallQueue;
        //    while (true)
        //    {
        //        try { 
        //        if (queue.Count == 0)
        //        {
        //            return;
        //        }
        //        Call call = null;
        //        bool b = queue.TryDequeue(out call);
        //        if (!b)
        //        {
        //            continue;
        //        }
        //        //if (call.Accepted)
        //        //{
        //        //    call = queue.Dequeue();
        //        //    CallData.GetInstance().SaveCall(call);
        //        //} else if (!call.Accepted && call.CancelledList.Count >= EmployeeFactory.GetInstance().GetAll().Count) {
        //        //    Recall(call);
        //        //}
        //        if (!call.Accepted && call.CancelledList.Count >= EmployeeFactory.GetInstance().GetAll().Count)
        //        {
        //            queue.Enqueue(call);
        //            Recall(call);
        //        }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //    }
        //}

        public static void ReceiveNewCall(Call call)
        {
            try { 
                CallFactory.GetInstance().Add(call.UniqueId, call);
                MobileTCPServer.Instance.SendToAll(ConnectionStrings.CALL + call.UniqueId + ":" + call.Room.Number + ":");
                Task.Run(() => CallData.GetInstance().SaveCall(call));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ReceiveNewTestCall(Call call)
        {
            try
            {
                CallFactory.GetInstance().Add(call.UniqueId, call);
                MobileTCPServer.Instance.SendToAll(ConnectionStrings.TESTCALL + call.UniqueId + ":" + call.Room.Number + ":");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ReceiveCallAcceptRespond(int employeeId, string uniqueId)
        {
            lock (thisLock)
            {
                try
                {              
                    Call call = CallFactory.GetInstance().GetByUniqueId(uniqueId);
                    if (call != null && !call.Accepted)
                    {
                        Employee e = EmployeeFactory.GetInstance().TryGet(employeeId);

                        call.Employee = e;
                        call.Accepted = true;
                        try
                        {
                            if (MobileTCPServer.Instance.Connections.ContainsKey(call.Employee.Id))
                            {
                                TcpServerConnection con;
                                bool b = MobileTCPServer.Instance.Connections.TryGetValue(call.Employee.Id, out con);
                                if (b)
                                {
                                    con.sendData(ConnectionStrings.CALL_APPROVED + uniqueId + ":");
                                }
                                else
                                {
                                    MobileTCPServer.Instance.MessageQueue.Enqueue(new MobileTCPServer.Message()
                                    {
                                        EmployeeId = call.Employee.Id,
                                        Msg = ConnectionStrings.CALL_APPROVED + uniqueId + ":"
                                    });
                                }
                            }
                            else {
                                MobileTCPServer.Instance.MessageQueue.Enqueue(new MobileTCPServer.Message() {
                                    EmployeeId = call.Employee.Id,
                                    Msg = ConnectionStrings.CALL_APPROVED + uniqueId + ":"
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(ex.StackTrace);
                            MobileTCPServer.Instance.MessageQueue.Enqueue(new MobileTCPServer.Message()
                            {
                                EmployeeId = call.Employee.Id,
                                Msg = ConnectionStrings.CALL_APPROVED + uniqueId + ":"
                            });
                        }
                        
                        CallFactory.GetInstance().TryRemove(call.UniqueId);

                        foreach (var item in MobileTCPServer.Instance.Connections)
                        {
                            if (item.Key == call.Employee.Id)
                            {
                                continue;
                            }
                            if (call.CancelledList.Contains(call.Employee.Id))
                            {
                                continue;
                            }
                            try
                            {
                                item.Value.sendData(ConnectionStrings.CALL_CANCELLED + uniqueId + ":");
                                //Console.WriteLine("#####################################################");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.WriteLine(ex.StackTrace);
                                MobileTCPServer.Instance.MessageQueue.Enqueue(new MobileTCPServer.Message()
                                {
                                    EmployeeId = item.Key,
                                    Msg = ConnectionStrings.CALL_CANCELLED + uniqueId + ":"
                                });
                            }
                           
                        }

                        foreach (var item in ExternalClientTCPServer.Instance.Connections)
                        {
                            try
                            {
                                item.Value.sendData("CALLACCEPTED:" + call.Room.Number + ":" + call.Employee.Name + ":\n");                               
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.WriteLine(ex.StackTrace);
                            }
                        }

                        Task.Run(() => CallData.GetInstance().UpdateCall(call));

                        List<Call> calls  = CallFactory.GetInstance().GetAll();
                        for (int i = 0; i < calls.Count; i++)
                        {
                            Call item = calls[i];
                            if (item.Room.Id == call.Room.Id)
                            {
                                CallFactory.GetInstance().TryRemove(item.UniqueId);
                                Task.Run(() => CallData.GetInstance().DeleteCall(item));
                            }
                        }                   
                    }
                    else
                    {
                        try
                        {
                            if (MobileTCPServer.Instance.Connections.ContainsKey(employeeId))
                            {
                                TcpServerConnection con;
                                bool b = MobileTCPServer.Instance.Connections.TryGetValue(employeeId, out con);
                                if (b)
                                {
                                    con.sendData(ConnectionStrings.CALL_REJECTED + uniqueId + ":");
                                }
                                else
                                {
                                    MobileTCPServer.Instance.MessageQueue.Enqueue(new MobileTCPServer.Message()
                                    {
                                        EmployeeId = call.Employee.Id,
                                        Msg = ConnectionStrings.CALL_REJECTED + uniqueId + ":"
                                    });
                                }
                            }
                            else {
                                MobileTCPServer.Instance.MessageQueue.Enqueue(new MobileTCPServer.Message()
                                {
                                    EmployeeId = call.Employee.Id,
                                    Msg = ConnectionStrings.CALL_REJECTED + uniqueId + ":"
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(ex.StackTrace);
                            MobileTCPServer.Instance.MessageQueue.Enqueue(new MobileTCPServer.Message()
                            {
                                EmployeeId = call.Employee.Id,
                                Msg = ConnectionStrings.CALL_REJECTED + uniqueId + ":"
                            });
                        }                  
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    MobileTCPServer.Instance.MessageQueue.Enqueue(new MobileTCPServer.Message()
                    {
                        EmployeeId = employeeId,
                        Msg = ConnectionStrings.ERROR
                    });               
                }
            }
        }

        public static void ReceiveCallCancelRespond(int employeeId, string uniqueId)
        {
            Call call = CallFactory.GetInstance().GetByUniqueId(uniqueId);
            if (call != null && !call.Accepted)
            {
                call.CancelledList.Add(employeeId);
                
                if (call.CancelledList.Count >= MobileTCPServer.Instance.Connections.Count)
                {
                    Recall(call);
                }
            }
        }

        private static void Recall(Call call)
        {
            call.CancelledList.Clear();
            call.TimeStamp = DateTime.Now;
            MobileTCPServer.Instance.SendToAll(ConnectionStrings.RECALL + call.UniqueId + ":" + call.Room.Number + ":");
        }
    }
}
