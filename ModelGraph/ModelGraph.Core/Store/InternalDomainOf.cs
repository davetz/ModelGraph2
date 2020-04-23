
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class InternalDomainOf<T> : StoreOf<T>, IDomain where T : Item
    {
        public InternalDomainOf(Chef owner, IdKey idKe, int capacity = 0) : base(owner, idKe, capacity) { }

        public int GetSerializerItemCount()
        {
            return 1 + Count; //===================count my self and my children
        }

        public void PopulateItemIndex(Dictionary<Item, int> itemIndex)
        {
            itemIndex[this] = 0; //====================enter my self
            foreach (var itm in Items)
            {
                if (itm.IsReference) itemIndex[itm] = 0; //================enter my children
            }
        }

        public void RegisterInternal(Dictionary<int, Item> internalItems)
        {
            internalItems.Add(ItemKey, this);
            foreach (var item in Items)
            {
                if (item.IsReference) internalItems.Add(item.ItemKey, item);
            }
        }
    }
}
