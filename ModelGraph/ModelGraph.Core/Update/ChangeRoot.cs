using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ChangeRoot : StoreOf<Change>
    {
        internal Change Change { get; private set; } //aggragates all changes made durring PostModelRequest(Action)

        internal static int ChangeSequence = 0;
        internal override IdKey IdKey => IdKey.ChangeRoot;


        private string _changeRootInfoText;
        private Item _changeRootInfoItem;
        private int _changeRootInfoCount;

        #region CongealChanges  ===============================================
        // Consolidate the current change items and freeze them so that the changes
        // can not be undone, also remove any change items wich have been undon. 
        public void CongealChanges()
        {
            var ChangeRoot = Get<ChangeRoot>();

            ChangeRoot.CongealChanges();
            ChangeRoot.ChildDelta++;
        }
        #endregion

        #region ItemRemoved  ==================================================
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
        #endregion

        #region ItemLinked  ===================================================
        internal void ItemLinked(Relation rel, Item item1, Item item2)
        {
            var nam1 = GetIdentity(item1, IdentityStyle.Double);
            var nam2 = GetIdentity(item2, IdentityStyle.Double);
            var rnam = GetIdentity(rel, IdentityStyle.Single);

            var name = $" [{rnam}]   ({nam1}) --> ({nam2})";
            (int parentIndex, int chilldIndex) = rel.AppendLink(item1, item2);
            new ItemLinked(Get<Change>(), rel, item1, item2, parentIndex, chilldIndex, name);
        }
        internal void Undo(ItemLinked chng)
        {
            chng.Relation.RemoveLink(chng.Parent, chng.Child);
        }

        internal void Redo(ItemLinked chng)
        {
            chng.Relation.AppendLink(chng.Parent, chng.Child);
        }

        #endregion

        #region ItemUnlinked  =================================================
        internal void MarkItemUnlinked(Relation rel, Item item1, Item item2)
        {
            (int parentIndex, int childIndex) = rel.GetIndex(item1, item2);
            if (parentIndex < 0 || childIndex < 0) return;

            var nam1 = GetIdentity(item1, IdentityStyle.Double);
            var nam2 = GetIdentity(item2, IdentityStyle.Double);
            var rnam = GetIdentity(rel, IdentityStyle.Single);

            var name = $" [{rnam}]   ({nam1}) --> ({nam2})";
            var chg = new ItemUnLinked(Get<Change>(), rel, item1, item2, parentIndex, childIndex, name);
        }

        internal void Redo(ItemUnLinked cg)
        {
            cg.Relation.RemoveLink(cg.Parent, cg.Child);
            cg.IsUndone = false;
        }

        internal void Undo(ItemUnLinked cg)
        {
            cg.Relation.InsertLink(cg.Parent, cg.Child, cg.ParentIndex, cg.ChildIndex);
            cg.IsUndone = true;
        }
        #endregion

        #region ItemChildMoved  ===============================================
        internal void ItemChildMoved(Relation relation, Item key, Item item, int index1, int index2)
        {
            var n1 = index1 + 1;
            var n2 = index2 + 1;
            var name = $" [{GetIdentity(relation, IdentityStyle.Single)}]     {GetIdentity(item, IdentityStyle.Double)}     {n1.ToString()}->{n2.ToString()}";
            var chg = new ItemChildMoved(Get<Change>(), relation, key, item, index1, index2, name);
            Redo(chg);
        }
        internal void Undo(ItemChildMoved chng)
        {
            chng.Relation.MoveChild(chng.Key, chng.Item, chng.Index1);
        }

        internal void Redo(ItemChildMoved chng)
        {
            chng.Relation.MoveChild(chng.Key, chng.Item, chng.Index2);
        }
        #endregion

        #region ItemParentMoved  ==============================================
        internal void ItemParentMoved(Relation relation, Item key, Item item, int index1, int index2)
        {
            var n1 = index1 + 1;
            var n2 = index2 + 1;
            var name = $" [{GetIdentity(relation, IdentityStyle.Single)}]     {GetIdentity(item, IdentityStyle.Double)}     {n1.ToString()}->{n2.ToString()}";
            var chg = new ItemParentMoved(Get<Change>(), relation, key, item, index1, index2, name);
            Redo(chg);
        }
        internal void Undo(ItemParentMoved chng)
        {
            chng.Relation.MoveChild(chng.Key, chng.Item, chng.Index1);
        }

        internal void Redo(ItemParentMoved chng)
        {
            chng.Relation.MoveChild(chng.Key, chng.Item, chng.Index2);
        }
        #endregion


        #region Constructor  ==================================================
        internal ChangeRoot(Chef chef)
        {
            Owner = chef;
            Change = new Change(this, ++ChangeSequence);

            chef.Add(this); // we must be in dataChef's item tree hierarchy
        }
        #endregion

        #region RecordChanges  ================================================
        /// <summary>Check for updates and save them</summary>
        internal void RecordChanges()
        {
            if (Change.Count > 0)
            {
                Add(Change);
                Change = new Change(this, ++ChangeSequence);
            }
        }
        #endregion

        #region Undo  =========================================================
        internal bool CanUndo(Change chg)
        {
            var last = Count - 1;
            for (int i = last; i >= 0; i--)
            {
                var item = Items[i];
                if (item == chg)
                    return item.CanUndo;
                else if (!item.CanRedo)
                    return false;
            }
            return false;
        }
        #endregion

        #region TryMerge  =====================================================
        internal void Mege(Change chg) => TryMerge(chg);
        internal bool CanMerge(Change chg) => TryMerge(chg, true);

        private bool TryMerge(Change cs, bool testIfCanMerge = false)
        {
            if (cs.IsCongealed) return false;

            var index = IndexOf(cs);
            if (index < 0) return false;
            var prev = index - 1;
            if (prev < 0) return false;

            var cs2 = Items[prev];
            if (cs2.IsCongealed) return false;
            if (cs2.IsUndone != cs.IsUndone) return false;

            if (testIfCanMerge) return true;

            cs.Owner.ModelDelta++;

            var items = cs2.Items;
            foreach (var item in items) { item.Reparent(cs); }
            Remove(cs2);

            //foreach (var cs3 in Items)
            //{
            //    cs3.ChildDelta++;
            //    cs3.ModelDelta++;
            //}
            return true;
        }
        #endregion

        #region CongealChanges  ===============================================
        internal void CongealChanges()
        {
            if (Count > 0)
            {
                ModelDelta++;
                ChildDelta++;
                Change save = null;
                var last = Count - 1;
                for (int i = last; i >= 0; i--)
                {
                    var cs = Items[i];
                    if (cs.IsCongealed) continue;
                    if (cs.IsUndone)
                        Remove(cs);
                    else if (save == null)
                        save = cs;
                }
                if (save != null)
                {
                    while (TryMerge(save)) { }
                    save.IsCongealed = true;
                }
            }
        }
        #endregion

        #region AutoExpandChanges  ============================================
        internal void AutoExpandChanges()
        {
            foreach (var chg in Items)
            {
                if (chg.IsCongealed) break;
                if (!chg.IsNew) break;
                chg.AutoExpandLeft = true;
                foreach (var item in chg.Items)
                {
                    item.AutoExpandLeft = true;
                }
            }
        }
        #endregion

        #region RemoveItem  ===================================================
        private void RemoveItem(Item target)
        {
            var chef = DataChef;
            var relItems = new Dictionary<Relation, Dictionary<Item, List<Item>>>();
            var hitList = new List<Item>();
            var stoCRels = chef.Get<Relation_Store_ChildRelation>();
            var stoPRels = chef.Get<Relation_Store_ParentRelation>();

            FindDependents(target, hitList, stoCRels);
            hitList.Reverse();

            foreach (var item in hitList)
            {
                if (item is Relation r)
                {
                    var N = r.GetLinks(out List<Item> parents, out List<Item> children);

                    for (int i = 0; i < N; i++) { TryMarkItemUnlinked(r, parents[i], children[i], relItems); }
                }
                if (TryGetParentRelations(item, out IList<Relation> relations, stoPRels))
                {
                    foreach (var rel in relations)
                    {
                        if (!rel.TryGetParents(item, out List<Item> parents)) continue;

                        foreach (var parent in parents) { TryMarkItemUnlinked(rel, parent, item, relItems); }
                    }
                }
                if (TryGetChildRelations(item, out relations, stoCRels))
                {
                    foreach (var rel in relations)
                    {
                        if (!rel.TryGetChildren(item, out List<Item> children)) continue;

                        foreach (var child in children) { TryMarkItemUnlinked(rel, item, child, relItems); }
                    }
                }
            }

            foreach (var item in hitList) { MarkItemRemoved(item); }
            Change.Redo();
        }
        #region PrivateMethods  ===========================================

        void FindDependents(Item target2, List<Item> hitList, Relation_Store_ChildRelation stoCRels)
        {
            hitList.Add(target2);
            if (target2 is Store store)
            {
                var items = store.GetItems();
                foreach (var item in items) FindDependents(item, hitList, stoCRels);
            }
            if (TryGetChildRelations(target2, out IList<Relation> relations, stoCRels))
            {
                foreach (var rel in relations)
                {
                    if (rel.IsRequired && rel.TryGetChildren(target2, out List<Item> children))
                    {
                        foreach (var child in children)
                        {
                            FindDependents(child, hitList, stoCRels);
                        }
                    }
                }
            }
        }

        bool TryGetChildRelations(Item item, out IList<Relation> relations, Relation_Store_ChildRelation stoCRels)
        {
            if (item.Owner is TableX tx)
            {
                if (stoCRels.TryGetChildren(tx, out IList<Relation> txRelations))
                {
                    relations = new List<Relation>(txRelations);
                    return true;
                }
                relations = null;
                return false;
            }
            return stoCRels.TryGetChildren(item.Owner, out relations);
        }

        bool TryGetParentRelations(Item item, out IList<Relation> relations, Relation_Store_ParentRelation stoPRels)
        {
            if (item.Owner is TableX tx)
            {
                if (stoPRels.TryGetChildren(tx, out IList<Relation> txRelations))
                {
                    relations = new List<Relation>(txRelations);
                    return true;
                }
                relations = null;
                return false;
            }
            return stoPRels.TryGetChildren(item.Owner, out relations);
        }

        bool TryMarkItemUnlinked(Relation rel, Item item1, Item item2, Dictionary<Relation, Dictionary<Item, List<Item>>> relItems)
        {
            List<Item> items;

            if (relItems.TryGetValue(rel, out Dictionary<Item, List<Item>> itemItems))
            {
                if (itemItems.TryGetValue(item1, out items))
                {
                    if (items.Contains(item2)) return false;
                    items.Add(item2);
                }
                else
                {
                    items = new List<Item>(2) { item2 };
                    itemItems.Add(item1, items);
                }
            }
            else
            {
                itemItems = new Dictionary<Item, List<Item>>(4);
                items = new List<Item>(2) { item2 };
                itemItems.Add(item1, items);
                relItems.Add(rel, itemItems);
            }
            MarkItemUnlinked(rel, item1, item2);
            return true;
        }
        #endregion
        #endregion
    }
}
