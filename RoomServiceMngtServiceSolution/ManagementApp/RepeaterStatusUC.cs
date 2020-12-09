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
    public partial class RepeaterStatusUC : UserControl
    {
        public RepeaterStatusUC()
        {
            InitializeComponent();
        }

        private void RepeaterStatusUC_Load(object sender, EventArgs e)
        {
            try
            {
                var Source = new BindingSource(RepeaterStatusFactory.Instance.StatusList, null);
                dataGridView1.DataSource = Source;
                if (NamedPipeConnector.IsConnected && NamedPipeConnector.Contract != null)
                    {
                        IList<RepeaterStatus> list = NamedPipeConnector.Contract.GetRepeaterStatus();
                        RepeaterStatusFactory.Instance.UpdateList(list);
                    }
                    else
                    {
                        bool connection = NamedPipeConnector.ConnectToService();
                        if (connection)
                        {
                            Console.WriteLine("Connecting to Service");
                            IList<RepeaterStatus> list = NamedPipeConnector.Contract.GetRepeaterStatus();
                            RepeaterStatusFactory.Instance.UpdateList(list);
                            Console.WriteLine("Connection Established with Service");
                        }
                        else
                        {
                        RepeaterStatusFactory.Instance.UpdateWaiting();
                        MessageBox.Show("Couldn't connect to server. Server might be stopped.", "Connection Error with Server", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                }
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                DataGridView dgv = sender as DataGridView;
                RepeaterStatus data = dgv.Rows[e.RowIndex].DataBoundItem as RepeaterStatus;

                if (data.Status.CompareTo("Running") == 0)
                {
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                    //e.CellStyle.BackColor = Color.Green;
                    e.CellStyle.ForeColor = Color.Green;
                }
                else if (data.Status.CompareTo("Off-line") == 0)
                {
                    //e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                    e.CellStyle.ForeColor = Color.Red;
                }
                else
                {
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                    e.CellStyle.ForeColor = Color.Orange;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
