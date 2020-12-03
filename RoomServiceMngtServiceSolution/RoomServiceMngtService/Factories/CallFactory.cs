using RoomServiceMngtService.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomServiceMngtService.Factories
{
    public class CallFactory : Factory<string, Call>
    {
        //public ConcurrentQueue<Call> CallQueue = new ConcurrentQueue<Call>();

        private static CallFactory Instance = new CallFactory();
        public static CallFactory GetInstance()
        {
            return Instance;
        }

        private CallFactory()
        {

        }

        public Call GetByUniqueId(string name)
        {
            foreach (Call v in Cache.Values)
            {
                if (v.UniqueId.CompareTo(name) == 0)
                {
                    return v;
                }
            }
            return null;
        }

        public void Add(string id, Call call)
        {
            base.TryAdd(id, call);
            //CallQueue.Enqueue(call);
        }
    }
}
