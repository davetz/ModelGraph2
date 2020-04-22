﻿using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class StoreOf<T> : Store where T : Item
    {
        private List<T> _items = new List<T>(0);    // list of child items

        #region Constructor  ==================================================
        public StoreOf() { }
        public StoreOf(Store owner, IdKey idKe, int capacity = 0)
        {
            Owner = owner;
            IdKey = idKe;
            SetCapacity(capacity);
            owner?.Add(this);
        }

        #endregion

        #region Count/Items/GetItems  =========================================
        internal IList<T> Items => _items.AsReadOnly(); // protected from accidental corruption
        internal override int Count => (_items == null) ? 0 : _items.Count;
        internal override List<Item> GetItems() => new List<Item>(_items);
        internal override void RemoveAll() { _items.Clear(); UpdateDelta(); }
        internal override int GetSerializerItemCount()
        {
            var count = 0;
            foreach (var item in Items)
            {
                if (item.IsExternal || item.IsReference) count++;
                if (item is Store sto) count += sto.GetSerializerItemCount();
            }
            return count;
        }
        internal override void PopululateChildItemIndex(Dictionary<Item, int> itemIndex)
        {
            foreach (var item in Items)
            {
                if (item.IsExternal) itemIndex[item] = 0;
                if (item is Store sto) sto.PopululateChildItemIndex(itemIndex);
            }
        }
        internal override Type GetChildType() => typeof(T);
        #endregion

        #region Methods  ======================================================
        private T Cast(Item item) => (item is T child) ? child : throw new InvalidCastException("StoreOf");
        private void UpdateDelta() { ModelDelta++; ChildDelta++; }

        internal void SetCapacity(int exactCount)
        {
            if (exactCount > 0)
            {
                var cap = (int)((exactCount + 1) * 1.1); // allow for modest expansion

                _items.Capacity = cap;
            }
        }

        // Add  =============================================================
        internal void Add(T item)
        {
            UpdateDelta();
            _items.Add(item);
        }
        internal override void Add(Item item) => Add(Cast(item));

        // Remove  ==========================================================
        internal void Remove(T item)
        {
            UpdateDelta();
            _items.Remove(item);
        }
        public override void Remove(Item item) => Remove(Cast(item));

        // Insert  ============================================================
        internal void Insert(T item, int index)
        {
            var i = (index < 0) ? 0 : index;

            UpdateDelta();
            if (i < _items.Count)
                _items.Insert(i, item);
            else
                _items.Add(item);
        }
        internal override void Insert(Item item, int index) => Insert(Cast(item), index); 

        // IndexOf  ===========================================================
        internal int IndexOf(T item) => _items.IndexOf(item);
        internal override int IndexOf(Item item) => IndexOf(Cast(item));

        // Move  ==============================================================
        internal void Move(T item, int index)
        {
            if (_items.Remove(item))
            {
                UpdateDelta();
                if (index < 0)
                    _items.Insert(0, item);
                else if (index < _items.Count)
                    _items.Insert(index, item);
                else
                    _items.Add(item);
            }
        }
        internal override void Move(Item item, int index) => Move(Cast(item), index);

        #endregion
    }
}
