﻿using System;

namespace ModelGraph.Core
{ 
    public class ItemParentMoved : ItemChange
    {
        internal Item Key;
        internal Item Item;
        internal Relation Relation;
        internal int Index1;
        internal int Index2;
       internal override IdKey IdKey => IdKey.ItemChildMoved;

    #region Constructor  ==================================================
    internal ItemParentMoved(ChangeSet owner, Relation relation, Item key, Item item, int index1, int index2, string name)
        {
            Owner = owner;
            _name = name;

            Key = key;
            Item = item;
            Relation = relation;
            Index1 = index1;
            Index2 = index2;

            owner.Add(this);
            UpdateDelta();
        }
        #endregion
    }
}
