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
    public class EmployeeService
    {
        public static void Initialize() {
            EmployeeData dao = EmployeeData.GetInstance();
            List<Employee> list = dao.GetAllEmployees();

            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            Console.WriteLine("Initializing Employees");
            foreach (var item in list)
            {
                Console.WriteLine(item.Name);
                EmployeeFactory.GetInstance().TryAdd(item.Id, item);
            }

            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
        }

        public static bool CheckEmplyeeAuth(string username, string password, out Employee emp) {
            emp = null;
            Employee employee =  EmployeeFactory.GetInstance().GetAll().Where(o => o.Username.CompareTo(username) == 0 && o.Password.CompareTo(password) == 0).FirstOrDefault();
            if (employee != null)
            {
                emp = employee;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
