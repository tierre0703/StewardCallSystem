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

            try
            {


                var _conn = DBManager.getCon();
                var _reader = DBManager.procGetRoomList();

                while (_reader.Read())
                {
                    roomList.Add(new Room
                    {
                        Id =  _reader.GetInt32(_reader.GetOrdinal("Id")),
                        UniqueId = _reader.GetString(_reader.GetOrdinal("UniqueId")),
                        Number = _reader.GetString(_reader.GetOrdinal("Number")),
                    });
                }

                DBManager.closeReader(_reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return roomList;
        }
    }
}
