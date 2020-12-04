using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace RoomServiceMngtService.DataAccess
{
    class DBManager
    {
        public static SQLiteConnection _conn = null;

        public static SQLiteConnection getCon()
        {
            if (_conn == null)
            {
                try
                {
                    _conn = new SQLiteConnection(Constants.DataBaseConnectionString);
                }
                catch (Exception e)
                {

                    _conn = null;
                }
            }
            return _conn;
        }

        public static void closeCon()
        {
            if (_conn != null)
                _conn.Close();
        }
    }
}
