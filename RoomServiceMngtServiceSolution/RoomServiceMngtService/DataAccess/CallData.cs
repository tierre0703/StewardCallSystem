//using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RoomServiceMngtService.Factories;
using RoomServiceMngtService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SQLite;

namespace RoomServiceMngtService.DataAccess
{
    public class CallData
    {
        private static CallData instance;

        private CallData() { }

        public static CallData GetInstance()
        {
            if (instance == null)
            {
                instance = new CallData();
            }
            return instance;
        }

        public string SaveCall(Call mObje)
        {
            string sucess = "";
            {
                try
                {
                    SQLiteConnection conn = DBManager.getCon();
                    DBManager.procInsertCall(mObje.UniqueId, 
                        /*mObje.Employee.Id,*/ 
                        mObje.Room.Id, 
                        Convert.ToInt32(mObje.Accepted), 
                        mObje.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss"));

                    sucess = "Sucess";
                }
                catch (Exception ex)
                {
                    sucess = "Fail";
                }
            }
            return sucess;
        }

        public string UpdateCall(Call mObje)
        {
            string sucess = "";
            try
            {
                SQLiteConnection conn = DBManager.getCon();
                DBManager.procUpdateCall(
                    mObje.UniqueId,
                    mObje.Employee.Id,
                    Convert.ToInt32(mObje.Accepted)
                    );

                sucess = "Sucess";
            }
            catch (Exception ex)
            {
                sucess = "Fail";
            }
            return sucess;
        }

        public string DeleteCall(Call mObje)
        {
            string sucess = "";
            try
            {
                
                SQLiteConnection conn = DBManager.getCon();
                DBManager.procDeleteCall(
                    mObje.UniqueId
                    );

                sucess = "Sucess";
            }
            catch (Exception ex)
            {
                sucess = "Fail";
            }
            return sucess;
        }

        public List<Call> GetDailyAcceptedCallsByEmployee(int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<Call> callList = new List<Call>();

            try
            {
                
                SQLiteConnection conn = DBManager.getCon();
                SQLiteDataReader _reader = DBManager.procGetDailyAcceptedCallsByEmployee(
                    employeeId,
                    fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    toDate.ToString("yyyy-MM-dd HH:mm:ss")
                    );

                while (_reader.Read())
                {
                    callList.Add(new Call
                    {
                        UniqueId = _reader.GetString(_reader.GetOrdinal("UniqueId")),
                        Room = RoomFactory.GetInstance().TryGet(_reader.GetInt32(_reader.GetOrdinal("RoomId"))),
                        TimeStamp = DateTime.Parse(_reader.GetString(_reader.GetOrdinal("TimeStamp"))),
                        Accepted = false,
                        Employee = ((_reader.IsDBNull(_reader.GetOrdinal("EmployeeId")))?null:EmployeeFactory.GetInstance().TryGet(_reader.GetInt32(_reader.GetOrdinal("EmployeeId"))))
                    });
                }
                DBManager.closeReader(_reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return callList;
        }

        public List<Call> GetRecentUnacceptedCalls(DateTime fromDate, DateTime toDate)
        {
            List<Call> callList = new List<Call>();

            try
            {
             
                var _conn = DBManager.getCon();
                var _reader = DBManager.procGetRecentUnacceptedCall(
                    fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    toDate.ToString("yyyy-MM-dd HH:mm:ss")
                    );
                while (_reader.Read())
                {
                    callList.Add(new Call
                    {
                        UniqueId = _reader.GetString(_reader.GetOrdinal("UniqueId")),
                        Room = RoomFactory.GetInstance().TryGet(_reader.GetInt32(_reader.GetOrdinal("RoomId"))),
                        TimeStamp = DateTime.Parse(_reader.GetString(_reader.GetOrdinal("TimeStamp"))),
                        Accepted = false,
                    });
                }

                DBManager.closeReader(_reader);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return callList;
        }
    }
}
