using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoomServiceMngtService.Common
{
    public class EmailSettings
    {
        public static string SMTP_HOST;
        public static int PORT;
        public static string USERNAME;
        public static string PASSWORD;

        public static string FROM_ADDRESS;
        public static string DISPLAY_NAME;

        public static List<string> TO_LIST = new List<string>();

        static EmailSettings() {
            string server = INIReader.ReadValue("smtp", "smtp_host", Path.GetDirectoryName(Application.ExecutablePath) + "\\Settings.ini");

            if (server == null || server.Length == 0)
            {
                Console.WriteLine("SMTP Host not valid");
                return;
            }
            SMTP_HOST = server;

            string port = INIReader.ReadValue("smtp", "port", Path.GetDirectoryName(Application.ExecutablePath) + "\\Settings.ini");

            if (port == null || port.Length == 0)
            {
                Console.WriteLine("Port not valid");
                return;
            }
            try
            {
                PORT = int.Parse(port);
            }
            catch (Exception e) {
                Console.WriteLine("Port not valid");
                return;
            }

            string username = INIReader.ReadValue("smtp", "username", Path.GetDirectoryName(Application.ExecutablePath) + "\\Settings.ini");

            if (username == null || username.Length == 0)
            {
                Console.WriteLine("Username not valid");
                return;
            }
            USERNAME = username;

            string password = INIReader.ReadValue("smtp", "password", Path.GetDirectoryName(Application.ExecutablePath) + "\\Settings.ini");

            if (password == null || password.Length == 0)
            {
                Console.WriteLine("Password not valid");
                return;
            }
            PASSWORD = password;

            string display_name = INIReader.ReadValue("from", "display_name", Path.GetDirectoryName(Application.ExecutablePath) + "\\Settings.ini");

            if (display_name == null || display_name.Length == 0)
            {
                Console.WriteLine("Display Name not valid");
                return;
            }
            DISPLAY_NAME = display_name;

            string from_addr = INIReader.ReadValue("from", "from_addr", Path.GetDirectoryName(Application.ExecutablePath) + "\\Settings.ini");

            if (from_addr == null || from_addr.Length == 0)
            {
                Console.WriteLine("From Address not valid");
                return;
            }
            FROM_ADDRESS = from_addr;

            TO_LIST.Clear();
            string[] values = INIReader.ReadKeys("to", Directory.GetCurrentDirectory() + "\\Settings.ini");
            if (values != null)
            {
                foreach (string v in values)
                {
                    string toaddress = INIReader.ReadValue("to", v, Directory.GetCurrentDirectory() + "\\Settings.ini");
                    if (toaddress != null && toaddress.Length != 0)
                    {
                        TO_LIST.Add(toaddress);
                    }
                    else
                    {
                        Console.WriteLine("To Address not valid");
                    }
                }
            }
            else
            {
            }
        }
    }
}
