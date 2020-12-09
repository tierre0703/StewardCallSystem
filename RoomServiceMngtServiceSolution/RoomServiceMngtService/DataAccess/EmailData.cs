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

            try
            {
                var _conn = DBManager.getCon();
                var _reader = DBManager.procGetEmailList();

                    while (_reader.Read())
                    {
                    employeeList.Add(new Email
                    {
                        Id = _reader.GetInt32(_reader.GetOrdinal("Id")),
                        EmailAddress = _reader.GetString(_reader.GetOrdinal("Email")),
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
