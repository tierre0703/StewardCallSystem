
using SharedContacts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagementApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            employeeMngtBtn_Click(this, null);
        }

        private void employeeMngtBtn_Click(object sender, EventArgs e)
        {
            employeeMngtBtn.BackColor = Color.FromArgb(84, 164, 226);
            employeeMngtBtn.ForeColor = Color.Black;

            roomMngtBtn.BackColor = Color.FromArgb(7, 72, 120);
            roomMngtBtn.ForeColor = Color.White;

            callBtn.BackColor = Color.FromArgb(7, 72, 120);
            callBtn.ForeColor = Color.White;

            mobileAppStatsBtn.BackColor = Color.FromArgb(7, 72, 120);
            mobileAppStatsBtn.ForeColor = Color.White;

            repeaterStatBtn.BackColor = Color.FromArgb(7, 72, 120);
            repeaterStatBtn.ForeColor = Color.White;

            btnEmail.BackColor = Color.FromArgb(7, 72, 120);
            btnEmail.ForeColor = Color.White;

            mainPanel.Controls.Clear();
            EmployeeMngtUC uc = new EmployeeMngtUC();
            mainPanel.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;
        }

        private void roomMngtBtn_Click(object sender, EventArgs e)
        {
            employeeMngtBtn.BackColor = Color.FromArgb(7, 72, 120);
            employeeMngtBtn.ForeColor = Color.White;

            roomMngtBtn.BackColor = Color.FromArgb(84, 164, 226);
            roomMngtBtn.ForeColor = Color.Black;

            callBtn.BackColor = Color.FromArgb(7, 72, 120);
            callBtn.ForeColor = Color.White;

            mobileAppStatsBtn.BackColor = Color.FromArgb(7, 72, 120);
            mobileAppStatsBtn.ForeColor = Color.White;

            repeaterStatBtn.BackColor = Color.FromArgb(7, 72, 120);
            repeaterStatBtn.ForeColor = Color.White;

            btnEmail.BackColor = Color.FromArgb(7, 72, 120);
            btnEmail.ForeColor = Color.White;

            mainPanel.Controls.Clear();
            RoomMngtUC uc = new RoomMngtUC();
            mainPanel.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;
        }

        private void callBtn_Click(object sender, EventArgs e)
        {
            employeeMngtBtn.BackColor = Color.FromArgb(7, 72, 120);
            employeeMngtBtn.ForeColor = Color.White;

            roomMngtBtn.BackColor = Color.FromArgb(7, 72, 120);
            roomMngtBtn.ForeColor = Color.White;

            callBtn.BackColor = Color.FromArgb(84, 164, 226);
            callBtn.ForeColor = Color.Black;

            mobileAppStatsBtn.BackColor = Color.FromArgb(7, 72, 120);
            mobileAppStatsBtn.ForeColor = Color.White;

            repeaterStatBtn.BackColor = Color.FromArgb(7, 72, 120);
            repeaterStatBtn.ForeColor = Color.White;

            btnEmail.BackColor = Color.FromArgb(7, 72, 120);
            btnEmail.ForeColor = Color.White;

            mainPanel.Controls.Clear();
            CallMngtUC uc = new CallMngtUC();
            mainPanel.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;
        }

        private void mobileAppStatsBtn_Click(object sender, EventArgs e)
        {
            employeeMngtBtn.BackColor = Color.FromArgb(7, 72, 120);
            employeeMngtBtn.ForeColor = Color.White;

            roomMngtBtn.BackColor = Color.FromArgb(7, 72, 120);
            roomMngtBtn.ForeColor = Color.White;

            callBtn.BackColor = Color.FromArgb(7, 72, 120);
            callBtn.ForeColor = Color.White;

            mobileAppStatsBtn.BackColor = Color.FromArgb(84, 164, 226);
            mobileAppStatsBtn.ForeColor = Color.Black;

            repeaterStatBtn.BackColor = Color.FromArgb(7, 72, 120);
            repeaterStatBtn.ForeColor = Color.White;

            btnEmail.BackColor = Color.FromArgb(7, 72, 120);
            btnEmail.ForeColor = Color.White;

            mainPanel.Controls.Clear();
            MobileAppStatusUC uc = new MobileAppStatusUC();
            mainPanel.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                List<Employee> emplist = EmployeeData.GetInstance().GetAllEmployees();
                List<MobileAppStatus> status = new List<MobileAppStatus>();
                foreach (var item in emplist)
                {
                    status.Add(new MobileAppStatus() { Employee = item.Name, EmployeeId = item.Id, Status = "Off-line"});
                }
                MobileAppStatusFactory.Instance.InitList(status);

                bool connection = NamedPipeConnector.ConnectToService();
                if (connection)
                {
                    Console.WriteLine("Connecting to Service");
                    IList<MobileAppStatus> list = NamedPipeConnector.Contract.GetMobileAppStatus();
                    MobileAppStatusFactory.Instance.UpdateList(list);

                    IList<RepeaterStatus> list2 = NamedPipeConnector.Contract.GetRepeaterStatus();
                    RepeaterStatusFactory.Instance.UpdateList(list2);
                    Console.WriteLine("Connection Established with Service");
                }
                else
                {
                    Console.WriteLine("Failed to Connect Service");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void repeaterStatBtn_Click(object sender, EventArgs e)
        {
            employeeMngtBtn.BackColor = Color.FromArgb(7, 72, 120);
            employeeMngtBtn.ForeColor = Color.White;

            roomMngtBtn.BackColor = Color.FromArgb(7, 72, 120);
            roomMngtBtn.ForeColor = Color.White;

            callBtn.BackColor = Color.FromArgb(7, 72, 120);
            callBtn.ForeColor = Color.White;

            mobileAppStatsBtn.BackColor = Color.FromArgb(7, 72, 120);
            mobileAppStatsBtn.ForeColor = Color.White;

            btnEmail.BackColor = Color.FromArgb(7, 72, 120);
            btnEmail.ForeColor = Color.White;

            repeaterStatBtn.BackColor = Color.FromArgb(84, 164, 226);
            repeaterStatBtn.ForeColor = Color.Black;

            mainPanel.Controls.Clear();
            RepeaterStatusUC uc = new RepeaterStatusUC();
            mainPanel.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            employeeMngtBtn.BackColor = Color.FromArgb(7, 72, 120);
            employeeMngtBtn.ForeColor = Color.White;

            roomMngtBtn.BackColor = Color.FromArgb(7, 72, 120);
            roomMngtBtn.ForeColor = Color.White;

            callBtn.BackColor = Color.FromArgb(7, 72, 120);
            callBtn.ForeColor = Color.White;

            mobileAppStatsBtn.BackColor = Color.FromArgb(7, 72, 120);
            mobileAppStatsBtn.ForeColor = Color.White;

            repeaterStatBtn.BackColor = Color.FromArgb(7, 72, 120);
            repeaterStatBtn.ForeColor = Color.White;

            btnEmail.BackColor = Color.FromArgb(84, 164, 226);
            btnEmail.ForeColor = Color.Black;

            mainPanel.Controls.Clear();
            EmailUC uc = new EmailUC();
            mainPanel.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;
        }
    }
}
