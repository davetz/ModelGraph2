
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class InternalStore<T> : StoreOf<T> where T : Item
    {
        public InternalStore(Chef owner, Trait trait, int capacity)
        {
            Owner = owner;
            Trait = trait;
            SetCapacity(capacity);

            owner.InternalStores.Add(this);  // collect by stores type
        }
        internal void RegisterInternal(Dictionary<int, Item> internalItems)
        {
            internalItems.Add(ItemKey, this);
            foreach (var item in Items)
            {
                if (item.IsInternal) internalItems.Add(item.ItemKey, item);
            }
        }
    }
}
