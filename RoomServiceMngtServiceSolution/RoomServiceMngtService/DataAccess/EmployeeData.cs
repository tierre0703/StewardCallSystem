using RoomServiceMngtService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Common;
using System.Data;

namespace RoomServiceMngtService.DataAccess
{
    public class EmployeeData
    {
        private static EmployeeData instance;

        private EmployeeData() { }

        public static EmployeeData GetInstance()
        {
            if (instance == null)
            {
                instance = new EmployeeData();
            }
            return instance;
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employeeList = new List<Employee>();

            using (TransactionScope mScop = new TransactionScope())
            {
                try
                {
                    Database db = new SqlDatabase(Constants.DataBaseConnectionString);
                    DbCommand dbCommand = db.GetStoredProcCommand("usp_GetEmployeeList");
                    using (IDataReader dr = db.ExecuteReader(dbCommand))
                    {
                        while (dr.Read())
                        {
                            employeeList.Add(new Employee
                            {
                                Id = int.Parse(dr["Id"].ToString()),
                                Name = dr["Name"].ToString(),                             
                                Username = dr["Username"].ToString(),
                                Password = dr["Password"].ToString(),               
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
