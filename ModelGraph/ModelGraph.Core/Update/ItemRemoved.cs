using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ItemRemoved : ItemChange
    {
        internal Item Item;
        internal int AtIndex;  // remember the item's location before it was removed
        internal IList<ColumnX> Columns; // remember the item,s column values (only applies to RowX items) 
        internal List<String> Values;
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


        internal void MarkItemRemoved(Item item)
        {
            if (!(item.Owner is Store sto)) return;

            var inx = (sto == null) ? -1 : sto.IndexOf(item);
            var name = GetIdentity(item, IdentityStyle.ChangeLog);

            if (item is RowX && Get<Relation_Store_ColumnX>().TryGetChildren(sto, out IList<ColumnX> cols))
            {
                var vals = new List<string>(cols.Count);
                foreach (var col in cols)
                {
                    vals.Add(col.Value.GetString(item));
                }
                new ItemRemoved(Get<Change>(), item, inx, name, cols, vals);
            }
            else
                new ItemRemoved(Get<Change>(), item, inx, name);
        }
        internal void Redo(ItemRemoved cg)
        {
            var itm = cg.Item;
            var sto = itm.Store;
            sto.Remove(itm);

            var N = (cg.Columns != null) ? cg.Columns.Count : 0;
            for (int i = 0; i < N; i++) { cg.Columns[i].Value.Remove(itm); }

            itm.IsDeleted = true;
            cg.IsUndone = false;
        }
        internal void Undo(ItemRemoved cg)
        {
            var itm = cg.Item;
            var sto = itm.Store;
            sto.Insert(itm, cg.AtIndex);

            var N = (cg.Columns != null) ? cg.Columns.Count : 0;
            for (int i = 0; i < N; i++) { cg.Columns[i].Value.SetString(itm, cg.Values[i]); }

            cg.Item.IsDeleted = false;
            cg.IsUndone = true;
        }

    }
}
