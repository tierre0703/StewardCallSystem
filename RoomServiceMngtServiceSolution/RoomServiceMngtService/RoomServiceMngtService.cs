using RoomServiceMngtService.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RoomServiceMngtService
{
    partial class RoomServiceMngtService : ServiceBase
    {
        private RepeaterTCPServer repeaterTCPServer;
        private MobileTCPServer mobileTCPServer;
        private ExternalClientTCPServer externalTCPServer;

        public RoomServiceMngtService()
        {
            InitializeComponent();

            
        }
        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }
        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.

            ServiceInitializer.InitializeAll();

            CallService.LoadRecentUnacceptedCalls();

            repeaterTCPServer = RepeaterTCPServer.Instance;
            repeaterTCPServer.Initialize();

            mobileTCPServer = MobileTCPServer.Instance;
            mobileTCPServer.Initialize();

            externalTCPServer = ExternalClientTCPServer.Instance;
            externalTCPServer.Initialize();

            //CallService.StartCallDatabaseService();

            CallService.StartUnacceptedCallCheck();

            NamedPipeServiceHost.StartServiceHost();

            EmailReportSchedule.Start();
        }

        protected override void OnStop()
        {
            repeaterTCPServer.StopAll();
            mobileTCPServer.StopAll();
            externalTCPServer.StopAll();
            //CallService.StopCallDatabaseService();
            CallService.StopUnacceptedCallCheck();
            NamedPipeServiceHost.StopServiceHost();
            EmailReportSchedule.Stop();
        }
    }
}
