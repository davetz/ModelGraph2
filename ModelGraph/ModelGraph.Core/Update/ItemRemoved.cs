using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ItemRemoved : ItemChange
    {
        internal Item Item;
        internal int AtIndex;  // preserve the item's location before it was removed
        internal IList<ColumnX> Columns; // preserve the item,s column values (only applies to RowX items) 
        internal List<string> Values;
        internal override IdKey IdKey => IdKey.ItemRemoved;

        #region Constructor  ==================================================
        internal ItemRemoved(Change owner, Item item, int index, string name, IList<ColumnX> columns = null, List<String> values = null)
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

        #region Record  =======================================================
        internal static void Record(Change owner, Root root, Item item, Relation_Store_ColumnX stoCols = null)
        {
            if (!(item.Owner is Store sto)) return;

            var inx = (sto == null) ? -1 : sto.IndexOf(item);
            var name = item.GetChangeLogId(root);

            if (stoCols.TryGetChildren(sto, out IList<ColumnX> cols))
            {
                var vals = new List<string>(cols.Count);
                foreach (var col in cols)
                {
                    vals.Add(col.Value.GetString(item));
                }
                new ItemRemoved(owner, item, inx, name, cols, vals);
            }
            else
                new ItemRemoved(owner, item, inx, name);
        }
        #endregion

        #region Undo/Redo  ====================================================
        override internal void Redo()
        {
            var itm = Item;
            var sto = itm.Store;
            sto.Remove(itm);

            var N = (Columns != null) ? Columns.Count : 0;
            for (int i = 0; i < N; i++) { Columns[i].Value.Remove(itm); }

            itm.IsDeleted = true;
            IsUndone = false;
        }

        override internal void Undo()
        {
            var itm = Item;
            var sto = Store;
            sto.Insert(itm, AtIndex);

            var N = (Columns != null) ? Columns.Count : 0;
            for (int i = 0; i < N; i++) { Columns[i].Value.SetString(itm, Values[i]); }

            Item.IsDeleted = false;
            IsUndone = true;
        }
        #endregion
    }
}
