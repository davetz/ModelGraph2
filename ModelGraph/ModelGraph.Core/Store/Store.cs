
using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class Store : Item
    {
        internal abstract string Name { get; set; }
        internal abstract string Summary { get; set; }
        internal abstract string Description { get; set; }
        internal virtual void RegisterInternal(Dictionary<int, Item> internalItem) { }
        internal abstract int GetSerializerItemCount();
        internal abstract void PopululateChildItemIndex(Dictionary<Item, int> itemIndex);
        internal abstract void Add(Item item);
        internal abstract void Move(Item item, int index);
        internal abstract void Insert(Item item, int index);
        public abstract void Remove(Item item);
        internal abstract void RemoveAll();
        internal abstract int IndexOf(Item item);
        internal abstract List<Item> GetItems();
        internal abstract int Count { get; }
        internal abstract Type GetChildType();

        internal bool TryLookUpProperty(string name, out Property property)
        {
            property = null;
            return GetChef().TryLookUpProperty(this, name, out property);
        }
    }
}
