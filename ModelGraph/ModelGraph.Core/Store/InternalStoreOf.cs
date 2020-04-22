
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class InternalStoreOf<T> : StoreOf<T> where T : Item
    {
        public InternalStoreOf(Chef owner, IdKey idKe, int capacity = 0) : base(owner, idKe, capacity) { }

        internal override void RegisterInternal(Dictionary<int, Item> internalItems)
        {
            internalItems.Add(ItemKey, this);
            foreach (var item in Items)
            {
                if (item.IsReference) internalItems.Add(item.ItemKey, item);
            }
        }
    }
}
