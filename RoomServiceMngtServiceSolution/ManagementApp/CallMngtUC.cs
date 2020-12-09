using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
//using System.Data.SqlClient;

using System.Configuration;
namespace ManagementApp
{
    public partial class CallMngtUC : UserControl
    {
        public static readonly string DataBaseConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        SQLiteConnection con;
        SQLiteDataAdapter adaptor;
        DataSet ds;
        SQLiteCommandBuilder sqlCommBuilder;

        public CallMngtUC()
        {
            InitializeComponent();

        }

        private void CallMngtUC_Load(object sender, EventArgs e)
        {
            try
            {
                con = DBManager.getCon(); //new SqlConnection();
                //con.ConnectionString = DataBaseConnectionString;

                //con.Open();

                DateTime fromDate = DateTime.Now;
                fromDate = fromDate.AddDays(-30);
                DateTime toDate = DateTime.Now;
                adaptor = new SQLiteDataAdapter("SELECT c.Id AS id_p, ROW_NUMBER() OVER(ORDER BY c.Id DESC) AS ID, e.Name AS 'Accepted User', r.Number AS Room, c.TimeStamp AS 'Call Time', c.ANSWERTimeStamp AS 'Answer Time' FROM Call c LEFT JOIN Employee e ON c.EmployeeId=e.Id INNER JOIN Room r ON c.RoomId=r.Id WHERE c.TimeStamp BETWEEN @fromDate AND @toDate ORDER BY c.Id DESC",
                    con);
                adaptor.SelectCommand.Parameters.AddWithValue("@fromDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));  //.Add(new SqlParameter("@fromDate", fromDate));
                adaptor.SelectCommand.Parameters.AddWithValue("@toDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));      //Add(new SqlParameter("@toDate", toDate));

                ds = new DataSet();
                adaptor.Fill(ds, "Call_Details");
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

        private void updateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                sqlCommBuilder = new SQLiteCommandBuilder(adaptor);
                adaptor.Update(ds, "Call_Details");

                MessageBox.Show("Successfully Updated!", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clearHistoryBtn_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete all the call history?",
                                     "Confirm Delete!!",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                //SqlConnection con1 = new SqlConnection();
                //con1.ConnectionString = DataBaseConnectionString;
                //con1.Open();
                SQLiteConnection con1 = DBManager.getCon();

                string query = ("DELETE FROM Call");
                SQLiteCommand command = new SQLiteCommand(query, con1);
                command.ExecuteNonQuery();
                //command.Dispose();
                //con1.Close();

                CallMngtUC_Load(this, null);
            }
            else
            {
                // If 'No', do something here.
            }
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
                        //sqlCommBuilder = new SqlCommandBuilder(adaptor);
                        sqlCommBuilder = new SQLiteCommandBuilder(adaptor);
                        adaptor.Update(ds, "Call_Details");

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

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {

            try
            {
                con = DBManager.getCon(); //new SqlConnection();
                //con.ConnectionString = DataBaseConnectionString;

                //con.Open();

                DateTime fromDate = DateTime.Now;
                fromDate = fromDate.AddDays(-30);
                DateTime toDate = DateTime.Now;
                adaptor = new SQLiteDataAdapter("SELECT c.Id AS id_p, ROW_NUMBER() OVER(ORDER BY c.Id DESC) AS ID, e.Name AS 'Accepted User', r.Number AS Room, c.TimeStamp AS 'Call Time', c.ANSWERTimeStamp AS 'Answer Time' FROM Call c LEFT JOIN Employee e ON c.EmployeeId=e.Id INNER JOIN Room r ON c.RoomId=r.Id WHERE c.TimeStamp BETWEEN @fromDate AND @toDate ORDER BY c.Id DESC",
                    con);
                adaptor.SelectCommand.Parameters.AddWithValue("@fromDate", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));  //.Add(new SqlParameter("@fromDate", fromDate));
                adaptor.SelectCommand.Parameters.AddWithValue("@toDate", toDate.ToString("yyyy-MM-dd HH:mm:ss"));      //Add(new SqlParameter("@toDate", toDate));

                ds = new DataSet();
                adaptor.Fill(ds, "Call_Details");
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Update();
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
    }
}
