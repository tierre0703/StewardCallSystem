using RoomServiceMngtService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomServiceMngtService.Factories
{
   public class EmployeeFactory : Factory<int, Employee>
    {
        private static EmployeeFactory Instance = new EmployeeFactory();
        public static EmployeeFactory GetInstance()
        {
            return Instance;
        }

        private EmployeeFactory()
        {

        }

        public Employee GetByName(string name)
        {
            foreach (Employee v in Cache.Values)
            {
                if (v.Name == name)
                {
                    return v;
                }
            }
            return null;
        }

        public Employee GetById(int Id)
        {
            return Cache.Values.Where(o => o.Id == Id).FirstOrDefault();
        }

        
    }

}
