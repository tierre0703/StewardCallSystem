using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

namespace ManagementApp
{
    public class Constants
    {
        static Constants()
        {

        }


        public static readonly string DataBaseConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        
    }
}
