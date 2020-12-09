using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.IO;

namespace ManagementApp
{
    public class Constants
    {
        static Constants()
        {

        }


        //public static readonly string DataBaseConnectionString// = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public static string DataBaseConnectionString// = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        {
            get
            {
                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                //string path = System.Environment.ExpandEnvironmentVariables("%AppData%\\StewardCallSystem\\");
                path += "\\StewardCallSystem\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path += "database.db";
                string connectionString = $"Data Source={path};Version=3;New=True;Compress=True;";
                return connectionString;
            }
        }


    }
}
