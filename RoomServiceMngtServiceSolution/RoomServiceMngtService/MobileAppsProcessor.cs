using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomServiceMngtService
{
    public class MobileAppsProcessor
    {
        private static object thisLock = new object();

        public static void ProcessMessage() {
            lock (thisLock)
            {
                
            }
        }
    }
}
