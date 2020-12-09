using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ManagementApp
{
    class DBManager
    {
        private static SQLiteConnection _conn = null;

        public static SQLiteConnection getCon()
        {
            if (_conn == null)
            {
                try
                {
                    _conn = new SQLiteConnection(Constants.DataBaseConnectionString);
                    _conn.Open();
                    createDb();
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
            {
                _conn.Close();
                _conn = null;
            }
        }

        public static void closeReader(SQLiteDataReader _reader)
        {
            if (_reader != null)
            {
                _reader.Close();
                _reader = null;
            }
        }

        public static void procDeleteCall(string _UniqueId)
        {
            try
            {
                if (_conn != null)
                {
                    using (SQLiteTransaction mytransaction = _conn.BeginTransaction())
                    {
                        SQLiteCommand cmd = new SQLiteCommand(_conn);
                        cmd.CommandText = $"DELETE FROM Call WHERE UniqueId = @UniqueId";
                        cmd.Parameters.AddWithValue("@UniqueId", _UniqueId);
                        cmd.Prepare();

                        cmd.ExecuteNonQuery();
                        mytransaction.Commit();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
        public static SQLiteDataReader procGetDailyAcceptedCallsByEmployee(int _EmployeeId, string _FromDate, string _ToDate)
        {
            SQLiteDataReader _reader = null;
            try
            {
                if (_conn != null)
                {
                    using (SQLiteTransaction mytransaction = _conn.BeginTransaction())
                    {
                        SQLiteCommand cmd = new SQLiteCommand(_conn);
                        cmd.CommandText = "SELECT Id, UniqueId, RoomId, TimeStamp, ANSWERTimeStamp, IsAccepted, EmployeeId FROM Call WHERE (EmployeeId = @EmployeeId) AND ([TimeStamp] BETWEEN @FromDate AND @ToDate) AND (IsAccepted=1)";
                        cmd.Parameters.AddWithValue("@EmployeeId", _EmployeeId);
                        cmd.Parameters.AddWithValue("@FromDate", _FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", _ToDate);
                        cmd.Prepare();
                        _reader = cmd.ExecuteReader();

                        mytransaction.Commit();
                    }
                }
            }
            catch (Exception e)
            { }

            return _reader;
        }

        public static SQLiteDataReader procGetEmailList()
        {
            SQLiteDataReader _reader = null;
            try
            {
                if (_conn != null)
                {
                    using (SQLiteTransaction mytransaction = _conn.BeginTransaction())
                    {
                        SQLiteCommand cmd = new SQLiteCommand(_conn);
                        cmd.CommandText = "SELECT Id, Email from Email2";
                        _reader = cmd.ExecuteReader();
                        mytransaction.Commit();
                    }
                }
            }
            catch (Exception e)
            { }
            return _reader;
        }

        public static void procUpdateCall(string _UniqueId, int _EmployeeId, int _IsAccepted)
        {
            try
            {
                if (_conn != null)
                {
                    using (SQLiteTransaction mytransaction = _conn.BeginTransaction())
                    {
                        SQLiteCommand cmd = new SQLiteCommand(_conn);
                        string time = "";
                        if (_IsAccepted == 1)
                        {
                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        cmd.CommandText = @"UPDATE Call SET EmployeeId= @EmployeeId, IsAccepted=@IsAccepted, ANSWERTimeStamp=@ANSWERTimeStamp WHERE UniqueId=@UniqueId";
                        cmd.Parameters.AddWithValue("@EmployeeId", _EmployeeId);
                        cmd.Parameters.AddWithValue("@IsAccepted", _IsAccepted);
                        cmd.Parameters.AddWithValue("@UniqueId", _UniqueId);
                        cmd.Parameters.AddWithValue("@ANSWERTimeStamp", time);
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();

                        mytransaction.Commit();
                    }
                }
            }
            catch (Exception e)
            { }
        }

        public static void procInsertCall(string _UniqueId, int _RoomId, int _IsAccepted, string _TimeStamp, string _AnswerTimeStamp)
        {
            try
            {
                if (_conn != null)
                {
                    using (SQLiteTransaction mytransaction = _conn.BeginTransaction())
                    {

                        SQLiteCommand cmd = new SQLiteCommand(_conn);
                        cmd.CommandText = @"insert into Call (UniqueId, RoomId, IsAccepted, TimeStamp, ANSWERTimeStamp)
                        values(@UniqueId, @RoomId, @IsAccepted, @TimeStamp, @ANSWERTimeStamp)";
                        cmd.Parameters.AddWithValue("@UniqueId", _UniqueId);
                        cmd.Parameters.AddWithValue("@RoomId", _RoomId);
                        cmd.Parameters.AddWithValue("@IsAccepted", _IsAccepted);
                        cmd.Parameters.AddWithValue("@TimeStamp", _TimeStamp);
                        cmd.Parameters.AddWithValue("@ANSWERTimeStamp", _AnswerTimeStamp);

                        cmd.Prepare();
                        Console.WriteLine(cmd.CommandText);
                        cmd.ExecuteNonQuery();
                        mytransaction.Commit();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }

        public static SQLiteDataReader procGetRoomList()
        {
            SQLiteDataReader _reader = null;
            try
            {
                if (_conn != null)
                {
                    using (SQLiteTransaction mytransaction = _conn.BeginTransaction())
                    {

                        SQLiteCommand cmd = new SQLiteCommand(_conn);
                        cmd.CommandText = "SELECT Id,UniqueId,Number from Room";
                        _reader = cmd.ExecuteReader();
                        mytransaction.Commit();
                    }
                }
            }
            catch (Exception e)
            {
            }

            return _reader;
        }

        public static SQLiteDataReader procGetRecentUnacceptedCall(string _FromDate, string _ToDate)
        {
            SQLiteDataReader _reader = null;
            try
            {
                if (_conn != null)
                {
                    using (SQLiteTransaction mytransaction = _conn.BeginTransaction())
                    {

                        SQLiteCommand cmd = new SQLiteCommand(_conn);
                        cmd.CommandText = "SELECT Id, UniqueId, RoomId, TimeStamp, ANSWERTimeStamp, IsAccepted FROM Call WHERE (TimeStamp BETWEEN @FromDate AND @ToDate) AND (IsAccepted=0) ";
                        cmd.Parameters.AddWithValue("@FromDate", _FromDate);
                        cmd.Parameters.AddWithValue("@ToDate", _ToDate);
                        cmd.Prepare();
                        _reader = cmd.ExecuteReader();
                        mytransaction.Commit();
                    }
                }
            }
            catch (Exception e)
            {
            }

            return _reader;
        }

        public static SQLiteDataReader procGetEmployeeList()
        {
            SQLiteDataReader _reader = null;
            try
            {
                if (_conn != null)
                {
                    using (SQLiteTransaction mytransaction = _conn.BeginTransaction())
                    {

                        SQLiteCommand cmd = new SQLiteCommand(_conn);
                        cmd.CommandText = "SELECT Id,Name,Username,Password from Employee";
                        _reader = cmd.ExecuteReader();
                        mytransaction.Commit();
                    }
                }
            }
            catch (Exception e)
            {
            }

            return _reader;
        }


        public static void createDb()
        {
            try
            {
                if (_conn != null)
                {
                    using (SQLiteTransaction mytransaction = _conn.BeginTransaction())
                    {

                        SQLiteCommand cmd = new SQLiteCommand(_conn);
                        cmd.CommandText =
                            @"CREATE TABLE IF NOT EXISTS Call (
                            Id    integer NOT NULL,
	                        UniqueId  nvarchar(50) COLLATE NOCASE,
                            EmployeeId    integer,
	                        RoomId    integer,
	                        IsAccepted    bit,
	                        TimeStamp nvarchar(50),
                            ANSWERTimeStamp nvarchar(50),
	                        FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
	                        FOREIGN KEY(RoomId) REFERENCES Room(Id),
	                        PRIMARY KEY(Id AUTOINCREMENT)
                        );
                        CREATE TABLE IF NOT EXISTS Email2(
                            Id    integer NOT NULL,
                            Email nvarchar(50) COLLATE NOCASE,
                            PRIMARY KEY(Id AUTOINCREMENT)
                        );
                        CREATE TABLE IF NOT EXISTS Employee(
                            Id    integer NOT NULL,
                            Name  nvarchar(50) COLLATE NOCASE,
                            Username  nvarchar(50) COLLATE NOCASE,
                            Password  nvarchar(50) COLLATE NOCASE,
                            PRIMARY KEY(Id AUTOINCREMENT)
                        );
                        CREATE TABLE IF NOT EXISTS Room(
                            Id    integer NOT NULL,
                            UniqueId  nvarchar(50) NOT NULL COLLATE NOCASE,
                            Number    nvarchar(50) COLLATE NOCASE,
                            PRIMARY KEY(Id AUTOINCREMENT)
                        );
                        CREATE UNIQUE INDEX IF NOT EXISTS Room_IX_Room ON Room(
                            Id    DESC
                        );
                        CREATE TRIGGER[fki_Call_EmployeeId_Employee_Id] Before Insert ON[Call] BEGIN SELECT RAISE(ROLLBACK, 'insert on table Call violates foreign key constraint fki_Call_EmployeeId_Employee_Id') WHERE NEW.EmployeeId IS NOT NULL AND(SELECT Id FROM Employee WHERE Id = NEW.EmployeeId) IS NULL; END;
                        CREATE TRIGGER[fku_Call_EmployeeId_Employee_Id] Before Update ON[Call] BEGIN SELECT RAISE(ROLLBACK, 'update on table Call violates foreign key constraint fku_Call_EmployeeId_Employee_Id') WHERE NEW.EmployeeId IS NOT NULL AND(SELECT Id FROM Employee WHERE Id = NEW.EmployeeId) IS NULL; END;
                        CREATE TRIGGER[fkd_Call_EmployeeId_Employee_Id] Before Delete ON[Employee] BEGIN SELECT RAISE(ROLLBACK, 'delete on table Employee violates foreign key constraint fkd_Call_EmployeeId_Employee_Id') WHERE(SELECT EmployeeId FROM Call WHERE EmployeeId = OLD.Id) IS NOT NULL; END;
                        CREATE TRIGGER[fki_Call_RoomId_Room_Id] Before Insert ON[Call] BEGIN SELECT RAISE(ROLLBACK, 'insert on table Call violates foreign key constraint fki_Call_RoomId_Room_Id') WHERE NEW.RoomId IS NOT NULL AND(SELECT Id FROM Room WHERE Id = NEW.RoomId) IS NULL; END;
                        CREATE TRIGGER[fku_Call_RoomId_Room_Id] Before Update ON[Call] BEGIN SELECT RAISE(ROLLBACK, 'update on table Call violates foreign key constraint fku_Call_RoomId_Room_Id') WHERE NEW.RoomId IS NOT NULL AND(SELECT Id FROM Room WHERE Id = NEW.RoomId) IS NULL; END;
                        CREATE TRIGGER[fkd_Call_RoomId_Room_Id] Before Delete ON[Room] BEGIN SELECT RAISE(ROLLBACK, 'delete on table Room violates foreign key constraint fkd_Call_RoomId_Room_Id') WHERE(SELECT RoomId FROM Call WHERE RoomId = OLD.Id) IS NOT NULL; END;
                        ";
                        cmd.ExecuteNonQuery();
                        mytransaction.Commit();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
