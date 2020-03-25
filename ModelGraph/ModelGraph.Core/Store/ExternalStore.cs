
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ExternalStore<T> : StoreOf<T> where T : Item
    {
        public ExternalStore(Chef owner, Trait trait, int capacity)
        {
            Owner = owner;
            Trait = trait;
            SetCapacity(capacity);

            owner.ExternalStores.Add(this);  // collect by stores type
        }
        internal void RegisterInternal(Dictionary<int, Item> internalItems)
        {
            internalItems.Add(ItemKey, this);
        }
        public bool HasData() => Count > 0;

        public void PopulateItemIndex(Dictionary<Item, int> itemIndex)
        {
            foreach (var item in Items)
            {
                itemIndex[item] = 0;
            }
        }
    }
}
