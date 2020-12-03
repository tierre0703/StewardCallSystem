using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RoomServiceMngtService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RoomServiceMngtService.DataAccess
{
    public class EmailData
    {
        private static EmailData instance;

        private EmailData() { }

        public static EmailData GetInstance()
        {
            if (instance == null)
            {
                instance = new EmailData();
            }
            return instance;
        }

        public List<Email> GetAllEmails()
        {
            List<Email> employeeList = new List<Email>();

            using (TransactionScope mScop = new TransactionScope())
            {
                try
                {
                    Database db = new SqlDatabase(Constants.DataBaseConnectionString);
                    DbCommand dbCommand = db.GetStoredProcCommand("usp_GetEmailList");
                    using (IDataReader dr = db.ExecuteReader(dbCommand))
                    {
                        while (dr.Read())
                        {
                            employeeList.Add(new Email
                            {
                                Id = int.Parse(dr["Id"].ToString()),
                                EmailAddress = dr["Email"].ToString(),
                            });
                        }
                    }
                    mScop.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return employeeList;
            }
        }
    }
}
