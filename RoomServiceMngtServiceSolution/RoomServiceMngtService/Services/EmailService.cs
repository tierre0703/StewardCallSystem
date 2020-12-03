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
    public class EmailService
    {
        public static void Initialize()
        {
            EmailData dao = EmailData.GetInstance();
            List<Email> list = dao.GetAllEmails();

            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            Console.WriteLine("Initializing Emails");
            foreach (var item in list)
            {
                Console.WriteLine(item.EmailAddress);
                EmailFactory.GetInstance().TryAdd(item.Id, item);
            }

            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
        }
    }
}
