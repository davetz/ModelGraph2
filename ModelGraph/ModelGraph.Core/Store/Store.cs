﻿
using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class Store : Item
    {
        internal abstract void Add(Item item);
        internal abstract void Move(Item item, int index);
        internal abstract void Insert(Item item, int index);
        public abstract void Remove(Item item);
        internal abstract void RemoveAll();
        internal abstract void Discard();
        internal abstract void DiscardChildren();
        internal abstract int IndexOf(Item item);
        internal abstract List<Item> GetItems();
        internal abstract int Count { get; }
        internal abstract Type GetChildType();

        internal bool TryLookUpProperty(string name, out Property property)
        {
            return DataChef.TryLookUpProperty(this, name, out property);
        }
    }
}
