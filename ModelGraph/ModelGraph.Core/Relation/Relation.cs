﻿using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class Relation : Item // used by undo/redo changes and StoreOf<Relation> _relationStore
    {
        private State _state;
        internal Pairing Pairing;

        #region State  ========================================================
        private bool GetFlag(State flag) => (_state & flag) != 0;
        private void SetFlag(State flag, bool value = true) { if (value) _state |= flag; else _state &= ~flag; }
        private enum State : ushort
        {
            IsRequired = 0x2, // Relation
        }

        internal bool IsRequired { get { return GetFlag(State.IsRequired); } set { SetFlag(State.IsRequired, value); } }

        internal override ushort GetState() => (ushort)_state;
        internal override void SetState(ushort val) => _state = (State)val;
        #endregion

        #region Serializer  ===================================================
        internal abstract (int, int)[] GetChildren1Items(Dictionary<Item, int> itemIndex);
        internal abstract (int, int)[] GetParent1Items(Dictionary<Item, int> itemIndex);
        internal abstract (int, int[])[] GetChildren2Items(Dictionary<Item, int> itemIndex);
        internal abstract (int, int[])[] GetParents2Items(Dictionary<Item, int> itemIndex);
        internal abstract void SetChildren1((int, int)[] items, Item[] itemArray);
        internal abstract void SetParents1((int, int)[] items, Item[] itemArray);
        internal abstract void SetChildren2((int, int[])[] items, Item[] itemArray);
        internal abstract void SetParents2((int, int[])[] items, Item[] itemArray);
        internal abstract bool HasLinks { get; }
        #endregion

        #region RequiredMethods  ==============================================
        internal bool HasNoParent(Item key)
        {
            return !HasParentLink(key);
        }
        internal bool HasNoChildren(Item key)
        {
            return !HasChildLink(key);
        }
        internal abstract bool HasChildLink(Item key);
        internal abstract bool HasParentLink(Item key);

        internal abstract bool TrySetPairing(Pairing pairing);
        internal abstract bool IsValidParentChild(Item parentItem, Item childItem);
        internal abstract int ChildCount(Item key);
        internal abstract int ParentCount(Item key);
        internal abstract bool RelationExists(Item key, Item childItem);
        internal abstract void InsertLink(Item parentItem, Item childItem, int parentIndex, int childIndex);
        internal abstract (int ParentIndex, int ChildIndex) AppendLink(Item parentItem, Item childItem);
        internal abstract (int ParentIndex, int ChildIndex) GetIndex(Item parentItem, Item childItem);
        internal abstract void RemoveLink(Item parentItem, Item childItem);
        internal abstract void MoveChild(Item key, Item item, int index);
        internal abstract void MoveParent(Item key, Item item, int index);
        internal abstract (int Index1, int Index2) GetChildrenIndex(Item key, Item item1, Item item2);
        internal abstract (int Index1, int Index2) GetParentsIndex(Item key, Item item1, Item item2);
        internal abstract int GetLinks(out List<Item> parents, out List<Item> children);
        internal abstract int GetLinksCount();
        internal abstract void SetLink(Item key, Item val, int capacity = 0); // used by storage file load
        internal abstract bool TryGetParents(Item key, out List<Item> parents);
        internal abstract bool TryGetChildren(Item key, out List<Item> children);
        internal abstract bool HasKey1(Item key);
        internal abstract bool HasKey2(Item key);
        internal abstract int KeyCount { get; }
        internal abstract int ValueCount { get; }
        #endregion
    }
}
