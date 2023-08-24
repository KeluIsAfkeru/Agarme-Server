using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agarme_Server.Misc
{
    /// <summary>
    /// 有较好性能的ID分配器
    /// </summary>
    public class IdAllocator
    {
        private uint nextId = 1;
        private LinkedList<uint> freeIds = new LinkedList<uint>();

        public uint Allocate()
        {
            if (freeIds.Count > 0)
            {
                var id = freeIds.First.Value;
                freeIds.RemoveFirst();
                return id;
            }
            else
            {
                if (nextId == int.MaxValue)
                {
                    throw new InvalidOperationException("No more IDs can be allocated.");
                }

                return nextId++;
            }
        }

        public void Recycle(uint id)
        {
            if (id < 0 || id >= nextId)
            {
                throw new ArgumentException("Invalid ID.");
            }

            freeIds.AddFirst(id);
        }

        public void Reset()
        {
            nextId = 0;
            freeIds.Clear();
        }
    }
}
