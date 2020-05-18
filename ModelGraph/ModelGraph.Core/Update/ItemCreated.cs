using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ItemCreated : ItemChange
    {
        internal Item Item;
        internal int AtIndex; // remember item's location in it's parent.Items list 
        internal override IdKey IdKey => IdKey.ItemCreated;

        #region Constructor  ==================================================
        private ItemCreated(Change owner, Item item, int index, string name)
        {
            Owner = owner;
            _name = name;
            Item = item;
            AtIndex = index;

            owner.Add(this);
            UpdateDelta();
        }
        #endregion

        #region Record  =======================================================
        /// <summary>Record the new Item created event</summary>
        internal static void Record(Chef chef, Item item)
        {
            var cs = chef.Get<ChangeRoot>().Change;

            string name = item.GetChangeLogId(chef);
            var store = item.Owner as Store;

            new ItemCreated(cs, item, store.IndexOf(item), name);
        }
        #endregion

        #region Undo  =========================================================
        internal override void Undo()
        {
            var item = Item;

            var store = item.Owner as Store;
            store.Remove(item);
        }
        #endregion

        #region Redo  =========================================================
        internal override void Redo()
        {
            var item = Item;
            var index = AtIndex;

            var store = item.Owner as Store;
            store.Insert(item, index);
        }
        #endregion
    }
}
