using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class StoreOf<T> : Store where T: Item
    {
        private List<T> _items = new List<T>(0);    // list of child items

        #region Constructor  ==================================================
        public StoreOf() { }
        public StoreOf(Chef owner, Trait trait, int capacity)
        {
            Owner = owner;
            Trait = trait;
            SetCapacity(capacity);

            owner?.Add(this); // we want this store to be in the dataChef's item tree hierarchy
        }
        internal override void Release()
        {
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

        internal override int GetSerializerCount()
        {
            var N = 0;
            foreach (var item in Items)
            {
                if (item.IsExternal || !item.IsTransient) N++;
                if (item is Store sto) N += sto.GetSerializerCount();
            }
            return N;
        }
        #endregion

        #region Flags  ========================================================
        // don't read/write missing or default-value propties
        // these flags indicate which properties were non-default
        public const byte BZ = 0;
        public const byte B1 = 0x1;
        public const byte B2 = 0x2;
        public const byte B3 = 0x4;
        public const byte B4 = 0x8;
        public const byte B5 = 0x10;
        public const byte B6 = 0x20;
        public const byte B7 = 0x40;
        public const byte B8 = 0x80;

        public const ushort SZ = 0;
        public const ushort S1 = 0x1;
        public const ushort S2 = 0x2;
        public const ushort S3 = 0x4;
        public const ushort S4 = 0x8;
        public const ushort S5 = 0x10;
        public const ushort S6 = 0x20;
        public const ushort S7 = 0x40;
        public const ushort S8 = 0x80;
        public const ushort S9 = 0x100;
        public const ushort S10 = 0x200;
        public const ushort S11 = 0x400;
        public const ushort S12 = 0x800;
        public const ushort S13 = 0x1000;
        public const ushort S14 = 0x2000;
        public const ushort S15 = 0x4000;
        public const ushort S16 = 0x8000;
        #endregion
    }
}
