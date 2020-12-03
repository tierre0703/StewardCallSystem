using RoomServiceMngtService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomServiceMngtService.Factories
{
    public class EmailFactory : Factory<int, Email>
    {
        //public ConcurrentQueue<Call> CallQueue = new ConcurrentQueue<Call>();

        private static EmailFactory Instance = new EmailFactory();
        public static EmailFactory GetInstance()
        {
            return Instance;
        }

        private EmailFactory()
        {

        }

        public void Add(int id, Email email)
        {
            base.TryAdd(id, email);
        }
    }
}
