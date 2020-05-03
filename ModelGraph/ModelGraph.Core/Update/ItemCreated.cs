﻿using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ItemCreated : ItemChange
    {
        internal List<String> Values;
        internal IList<ColumnX> Columns; // remember the item,s column values (only applies to RowX items) 

        internal Item Item;
        internal int AtIndex; // remember item's location in the Items list 
        internal override IdKey ViKey => IdKey.ItemCreated;


        #region Constructor  ==================================================
        internal ItemCreated(ChangeSet owner, Item item, int index, string name, IList<ColumnX> columns = null, List<String> values = null)
        {
            Owner = owner;
            _name = name;
            Item = item;
            AtIndex = index;
            Columns = columns;
            Values = values;

            owner.Add(this);
            UpdateDelta();
        }
        #endregion

    }
}
