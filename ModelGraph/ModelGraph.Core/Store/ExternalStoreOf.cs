
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ExternalStoreOf<T> : StoreOf<T> where T : Item
    {
        public ExternalStoreOf(Chef owner, Trait trait, int capacity = 0) : base(owner, trait, capacity) { }

        internal override bool HasDesendantCount => true;

        public bool HasData() => Count > 0;
        public void PopulateItemIndex(Dictionary<Item, int> itemIndex)
        {
            PopululateChildItemIndex(itemIndex);
        }
        internal override void RegisterInternal(Dictionary<int, Item> internalItems)
        {
            internalItems.Add(ItemKey, this);
        }
    }
}
