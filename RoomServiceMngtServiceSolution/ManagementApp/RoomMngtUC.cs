using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace ManagementApp
{
    public partial class RoomMngtUC : UserControl
    {
        public static readonly string DataBaseConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        SqlConnection con;
        SqlDataAdapter adaptor;
        DataSet ds;
        SqlCommandBuilder sqlCommBuilder;

        public RoomMngtUC()
        {
            InitializeComponent();
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                sqlCommBuilder = new SqlCommandBuilder(adaptor);
                adaptor.Update(ds, "Room_Details");

                ServiceHandler.ServiceResart();
                MessageBox.Show("Successfully Updated!", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RoomMngtUC_Load(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection();
                con.ConnectionString = DataBaseConnectionString;

                con.Open();

                adaptor = new SqlDataAdapter("SELECT Id AS id_p, ROW_NUMBER() OVER(ORDER BY Id)  AS ID,  UniqueId AS 'Unique ID', Number AS 'Room Name' FROM Room", con);
                ds = new DataSet();
                adaptor.Fill(ds, "Room_Details");
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (dataGridView1.ColumnCount > 0)
            {
                dataGridView1.Columns[0].Visible = false;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount =
        dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete selected records ?",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
                    {
                        dataGridView1.Rows.RemoveAt(item.Index);
                    }

                    try
                    {
                        sqlCommBuilder = new SqlCommandBuilder(adaptor);
                        adaptor.Update(ds, "Room_Details");

                        ServiceHandler.ServiceResart();
                        MessageBox.Show("Successfully Deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {

                }
            }
            else
            {
                MessageBox.Show("No records selected!", "Empty Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
