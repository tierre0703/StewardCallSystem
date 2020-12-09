using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomServiceMngtService.Model
{
    public class Call
    {
        public string UniqueId;
        public Room Room;
        public Employee Employee;
        public bool Accepted;
        public DateTime TimeStamp;
        public string ANSWERTimeStamp;
        public List<int> CancelledList = new List<int>();
    }
}
