using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Windows.Forms;

namespace ManagementApp
{
    public class ServiceHandler
    {

        private static ServiceController Service;

        public static void ServiceResart()
        {
           
                Service = new ServiceController("StewardCallSystemService");


                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(30000);

            try
            {
                if (Service.Status == ServiceControllerStatus.Running)
                {
                    Service.Stop();
                    Service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
            //Thread.Sleep(5000);
           

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(30000 - (millisec2 - millisec1));

            try
            {
                Service.Start();
                Service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }

        }
    }
}
