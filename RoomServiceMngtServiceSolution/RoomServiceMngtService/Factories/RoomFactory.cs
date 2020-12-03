using RoomServiceMngtService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomServiceMngtService.Factories
{
    public class RoomFactory : Factory<int, Room>
    {
        private static RoomFactory Instance = new RoomFactory();
        public static RoomFactory GetInstance()
        {
            return Instance;
        }

        private RoomFactory()
        {

        }

        public Room GetByUniqueId(string name)
        {
            foreach (Room v in Cache.Values)
            {
                if (v.UniqueId.CompareTo(name) == 0)
                {
                    return v;
                }
            }
            return null;
        }

        public Room GetById(int Id)
        {
            return Cache.Values.Where(o => o.Id == Id).FirstOrDefault();
        }


    }
}
