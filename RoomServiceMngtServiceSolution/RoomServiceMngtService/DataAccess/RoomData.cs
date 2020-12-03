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
    public class RoomData
    {
        private static RoomData instance;

        private RoomData() { }

        public static RoomData GetInstance()
        {
            if (instance == null)
            {
                instance = new RoomData();
            }
            return instance;
        }

        public List<Room> GetAllRooms()
        {
            List<Room> roomList = new List<Room>();

            using (TransactionScope mScop = new TransactionScope())
            {
                try
                {
                    Database db = new SqlDatabase(Constants.DataBaseConnectionString);
                    DbCommand dbCommand = db.GetStoredProcCommand("usp_GetRoomList");
                    using (IDataReader dr = db.ExecuteReader(dbCommand))
                    {
                        while (dr.Read())
                        {
                            roomList.Add(new Room
                            {
                                Id = int.Parse(dr["Id"].ToString()),
                                UniqueId = dr["UniqueId"].ToString(),
                                Number = dr["Number"].ToString(),
                            });
                        }
                    }
                    mScop.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return roomList;
            }

        }
    }
}
