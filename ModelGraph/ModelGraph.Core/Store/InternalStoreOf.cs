
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class InternalStoreOf<T> : StoreOf<T> where T : Item
    {
        public InternalStoreOf(Chef owner, Trait trait, int capacity = 0) : base(owner, trait, capacity) { }

        internal override bool HasDesendantCount => true;

        internal override void RegisterInternal(Dictionary<int, Item> internalItems)
        {
            internalItems.Add(ItemKey, this);
            foreach (var item in Items)
            {
                if (item.IsInternal) internalItems.Add(item.ItemKey, item);
            }
        }
    }
}
