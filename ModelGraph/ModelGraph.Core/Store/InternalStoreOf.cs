
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class InternalStoreOf<T> : StoreOf<T> where T : Item
    {
        internal override bool IsReference => true;
        public InternalStoreOf(Chef owner, Trait trait, int capacity = 0) : base(owner, trait, capacity) { }

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
