using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
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
            using (TransactionScope mScop = new TransactionScope())
            {
                try
                {
                    Database db = new SqlDatabase(Constants.DataBaseConnectionString);
                    DbCommand dbCommand = db.GetStoredProcCommand("usp_InsertCall");
                    db.AddInParameter(dbCommand, "@UniqueId", DbType.String, mObje.UniqueId);
                    //db.AddInParameter(dbCommand, "@EmployeeId", DbType.Int32, mObje.Employee.Id);
                    db.AddInParameter(dbCommand, "@RoomId", DbType.Int32, mObje.Room.Id);
                    db.AddInParameter(dbCommand, "@IsAccepted", DbType.Boolean, mObje.Accepted);
                    db.AddInParameter(dbCommand, "@TimeStamp", DbType.DateTime, mObje.TimeStamp);

                    db.ExecuteNonQuery(dbCommand);

                    mScop.Complete();

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
            using (TransactionScope mScop = new TransactionScope())
            {
                try
                {
                    Database db = new SqlDatabase(Constants.DataBaseConnectionString);
                    DbCommand dbCommand = db.GetStoredProcCommand("usp_UpdateCall");
                    db.AddInParameter(dbCommand, "@UniqueId", DbType.String, mObje.UniqueId);
                    db.AddInParameter(dbCommand, "@EmployeeId", DbType.Int32, mObje.Employee.Id);
                    //db.AddInParameter(dbCommand, "@RoomId", DbType.Int32, mObje.Room.Id);
                    db.AddInParameter(dbCommand, "@IsAccepted", DbType.Boolean, mObje.Accepted);
                    //db.AddInParameter(dbCommand, "@TimeStamp", DbType.DateTime, mObje.TimeStamp);

                    db.ExecuteNonQuery(dbCommand);

                    mScop.Complete();

                    sucess = "Sucess";
                }
                catch (Exception ex)
                {
                    sucess = "Fail";
                }
            }
            return sucess;
        }

        public string DeleteCall(Call mObje)
        {
            string sucess = "";
            using (TransactionScope mScop = new TransactionScope())
            {
                try
                {
                    Database db = new SqlDatabase(Constants.DataBaseConnectionString);
                    DbCommand dbCommand = db.GetStoredProcCommand("usp_DeleteCall");
                    db.AddInParameter(dbCommand, "@UniqueId", DbType.String, mObje.UniqueId);

                    db.ExecuteNonQuery(dbCommand);

                    mScop.Complete();

                    sucess = "Sucess";
                }
                catch (Exception ex)
                {
                    sucess = "Fail";
                }
            }
            return sucess;
        }

        public List<Call> GetDailyAcceptedCallsByEmployee(int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<Call> callList = new List<Call>();

            using (TransactionScope mScop = new TransactionScope())
            {
                try
                {
                    Database db = new SqlDatabase(Constants.DataBaseConnectionString);
                    DbCommand dbCommand = db.GetStoredProcCommand("usp_GetDailyAcceptedCallsByEmployee");
                    db.AddInParameter(dbCommand, "@EmployeeId", DbType.Int32, employeeId);
                    db.AddInParameter(dbCommand, "@FromDate", DbType.DateTime, fromDate);
                    db.AddInParameter(dbCommand, "@ToDate", DbType.DateTime, toDate);

                    using (IDataReader dr = db.ExecuteReader(dbCommand))
                    {
                        while (dr.Read())
                        {
                            callList.Add(new Call
                            {
                                UniqueId = dr["UniqueId"].ToString(),
                                Room = RoomFactory.GetInstance().TryGet(int.Parse(dr["RoomId"].ToString())),
                                TimeStamp = DateTime.Parse(dr["TimeStamp"].ToString()),
                                Accepted = false,
                                Employee = ((dr["EmployeeId"] == null)?null:EmployeeFactory.GetInstance().TryGet(int.Parse(dr["EmployeeId"].ToString())))
                            });
                        }
                    }
                    mScop.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return callList;
            }
        }

        public List<Call> GetRecentUnacceptedCalls(DateTime fromDate, DateTime toDate)
        {
            List<Call> callList = new List<Call>();

            using (TransactionScope mScop = new TransactionScope())
            {
                try
                {
                    Database db = new SqlDatabase(Constants.DataBaseConnectionString);
                    DbCommand dbCommand = db.GetStoredProcCommand("usp_GetRecentUnacceptedCall");
                    db.AddInParameter(dbCommand, "@FromDate", DbType.DateTime,fromDate);
                    db.AddInParameter(dbCommand, "@ToDate", DbType.DateTime, toDate);

                    using (IDataReader dr = db.ExecuteReader(dbCommand))
                    {
                        while (dr.Read())
                        {
                            callList.Add(new Call
                            {
                                UniqueId = dr["UniqueId"].ToString(),
                                Room = RoomFactory.GetInstance().TryGet(int.Parse(dr["RoomId"].ToString())),
                                TimeStamp = DateTime.Parse(dr["TimeStamp"].ToString()),
                                Accepted = false,
                            });
                        }
                    }
                    mScop.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return callList;
            }
        }
    }
}
