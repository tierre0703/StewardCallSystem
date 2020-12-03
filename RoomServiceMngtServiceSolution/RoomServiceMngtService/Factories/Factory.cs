using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomServiceMngtService.Factories
{
    public class Factory<T, E>
    {
        protected ConcurrentDictionary<T, E> Cache = new ConcurrentDictionary<T, E>();

        public bool TryAdd(T id, E category)
        {
            return Cache.TryAdd(id, category);
        }

        public E TryGet(T id)
        {
            E e;
            Cache.TryGetValue(id, out e);
            return e;
        }

        public List<E> GetAll()
        {
            return Cache.Values.ToList();
        }
        public bool TryRemove(T id)
        {
            E e;
            return Cache.TryRemove(id, out e);
        }
        public void Clear()
        {
            Cache.Clear();
        }
    }
}
