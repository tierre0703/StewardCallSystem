using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomServiceMngtService.Services
{
    public class ServiceInitializer
    {
        public static void InitializeAll() {
            EmployeeService.Initialize();
            RoomService.Initialize();
            EmailService.Initialize();
        }
    }
}
