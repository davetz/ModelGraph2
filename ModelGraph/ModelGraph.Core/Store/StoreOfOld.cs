﻿using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class StoreOfOld<T> : Store where T: Item
    {
        private List<T> _items = new List<T>(0);    // list of child items
        internal Guid Guid; // all stores have a Guid

        #region Constructor  ==================================================
        public StoreOfOld() { }
        public StoreOfOld(Chef owner, Trait trait, Guid guid, int capacity)
        {
            Owner = owner;
            Trait = trait;
            Guid = guid;
            SetCapacity(capacity);

            owner?.Add(this); // we want this store to be in the dataChef's item tree hierarchy
        }
        internal override void Release()
        {
            if (Count > 0)
            {
                foreach (var item in _items)
                {
                    item.Release();
                }
                _items = null;
            }
            base.Release();
        }
        #endregion

        #region Count/Items/ItemsReversed  ====================================
        internal IList<T> Items => _items.AsReadOnly(); // protected from accidental corruption
        internal override int Count => (_items == null) ? 0 : _items.Count;
        internal override List<Item> GetItems() => new List<Item>(_items);
        internal override void RemoveAll() { _items.Clear(); UpdateDelta(); }
        #endregion

        #region Methods  ======================================================
        private T Cast(Item item) => (item is T child) ? child : throw new InvalidCastException("StoreOf");
        private void UpdateDelta() { ModelDelta++; ChildDelta++; }

        internal void SetCapacity(int exactCount)
        {
            var cap = (int)((exactCount + 1) * 1.1); // allow for modest expansion

            _items.Capacity = cap;
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
