using RoomServiceMngtService.DataAccess;
using RoomServiceMngtService.Factories;
using RoomServiceMngtService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomServiceMngtService.Services
{
    public class RoomService
    {
        public static void Initialize()
        {
            string uniqueId = Guid.NewGuid().ToString();
            RoomData dao = RoomData.GetInstance();
            List<Room> list = dao.GetAllRooms();

            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            Console.WriteLine("Initializing Rooms");
            foreach (var item in list)
            {
                Console.WriteLine(item.Number);
                RoomFactory.GetInstance().TryAdd(item.Id, item);
            }

            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
        }
    }
}
