using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedContacts;

namespace ManagementApp
{
    public partial class MobileAppStatusUC : UserControl
    {


        public MobileAppStatusUC()
        {
            InitializeComponent();
            
        }

        private void MobileAppStatusUC_Load(object sender, EventArgs e)
        {
            try
            {
                
                if (NamedPipeConnector.IsConnected && NamedPipeConnector.Contract != null)
                    {
                        IList<MobileAppStatus> list = NamedPipeConnector.Contract.GetMobileAppStatus();
                        MobileAppStatusFactory.Instance.UpdateList(list);
                    }
                    else
                    {
                        bool connection = NamedPipeConnector.ConnectToService();
                        if (connection)
                        {
                            Console.WriteLine("Connecting to Service");
                            IList<MobileAppStatus> list = NamedPipeConnector.Contract.GetMobileAppStatus();
                            MobileAppStatusFactory.Instance.UpdateList(list);
                            Console.WriteLine("Connection Established with Service");
                    }else
                    {
                        MobileAppStatusFactory.Instance.UpdateWaiting();
                        MessageBox.Show("Couldn't connect to server. Server might be stopped.", "Connection Error with Server", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }
                    }

                var Source = new BindingSource(MobileAppStatusFactory.Instance.StatusList, null);
                dataGridView1.DataSource = Source;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            PDFExport.Export(dataGridView1, "Mobile Application Status Report");
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                DataGridView dgv = sender as DataGridView;
                MobileAppStatus data = dgv.Rows[e.RowIndex].DataBoundItem as MobileAppStatus;

                if (data.Status.CompareTo("Running") == 0)
                {
                    //e.CellStyle.BackColor = Color.Green;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                    e.CellStyle.ForeColor = Color.Green;
                }
                else if (data.Status.CompareTo("Off-line") == 0)
                {
                    //e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                    e.CellStyle.ForeColor = Color.Red;
                }
                else {
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                    e.CellStyle.ForeColor = Color.Orange;
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
