﻿using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ChangeRoot : StoreOf<Change>
    {
        internal Change Change { get; private set; } //aggragates all changes made durring ModelRequest(Action)
        private string _infoText;
        private Item _infoItem;
        private int _infoCount;

        internal override IdKey IdKey => IdKey.ChangeRoot;

        #region Constructor  ==================================================
        internal ChangeRoot(Root root)
        {
            Owner = root;
            Change = new Change(this);

            root.Add(this); // add myself to the dataChef's item tree hierarchy
        }
        #endregion

        #region RecordChanges  ================================================
        /// <summary>Check for updates and save them</summary>
        internal void RecordChanges()
        {
            if (Change.Count > 0)
            {
                Add(Change);
                Change = new Change(this);
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
            var root = DataChef;// big daddy
            var hitList = new List<Item>();//======================== dependant items that also need to be killed off
            var stoCRels = root.Get<Relation_Store_ChildRelation>();//==== souce1 of relational integrity
            var stoPRels = root.Get<Relation_Store_ParentRelation>();//==== souce2 of relational integrity
            var stoCols = root.Get<Relation_Store_ColumnX>(); //======= reference to user created columns
            var history = new Dictionary<Relation, Dictionary<Item, List<Item>>>(); //history of unlinked relationships

            FindDependents(target, hitList, stoCRels);
            hitList.Reverse();

            foreach (var item in hitList)
            {
                if (item is Relation r)
                {
                    var N = r.GetLinks(out List<Item> parents, out List<Item> children);

                    for (int i = 0; i < N; i++) { ItemUnLinked.Record(Change, root, r, parents[i], children[i], history); }
                }
                if (TryGetParentRelations(item, out IList<Relation> relations, stoPRels))
                {
                    foreach (var rel in relations)
                    {
                        if (!rel.TryGetParents(item, out List<Item> parents)) continue;

                        foreach (var parent in parents) { ItemUnLinked.Record(Change, root, rel, parent, item, history); }
                    }
                }
                if (TryGetChildRelations(item, out relations, stoCRels))
                {
                    foreach (var rel in relations)
                    {
                        if (!rel.TryGetChildren(item, out List<Item> children)) continue;

                        foreach (var child in children) { ItemUnLinked.Record(Change, root, rel, item, child, history); }
                    }
                }
            }

            foreach (var item in hitList) { ItemRemoved.Record(Change, root, item); }

            Change.Redo(); //now finally do all changes in the correct order
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

        #endregion
        #endregion
    }
}
