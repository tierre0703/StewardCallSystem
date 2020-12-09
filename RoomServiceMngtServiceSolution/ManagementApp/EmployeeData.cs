
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Common;
using System.Data;
using System.Data.SQLite;

namespace ManagementApp
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

            try
            {
                var conn = DBManager.getCon();
                var _reader  = DBManager.procGetEmployeeList();
                while (_reader.Read())
                {
                    employeeList.Add(new Employee
                    {
                        Id = _reader.GetInt32(_reader.GetOrdinal("Id")),
                        Name = _reader.GetString(_reader.GetOrdinal("Name")),                             
                        Username = _reader.GetString(_reader.GetOrdinal("Username")),
                        Password = _reader.GetString(_reader.GetOrdinal("Password")),               
                    });
                }

                DBManager.closeReader(_reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return employeeList;
        }
    }
}
