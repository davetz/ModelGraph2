﻿namespace ModelGraph.Core
{/*

 */
    public class ItemUnLinked : ItemChange
    {
        internal Item Child;
        internal Item Parent;
        internal Relation Relation;
        internal int ChildIndex;
        internal int ParentIndex;

        #region Constructor  ==================================================
        internal ItemUnLinked(ChangeSet owner, Relation relation, Item parent, Item child, int parentIndex, int childIndex, string name)
        {
            Owner = owner;
            Trait = IdKey.ItemUnlinked;
            Name = name;

            Child = child;
            Parent = parent;
            Relation = relation;
            ChildIndex = parentIndex;
            ParentIndex = parentIndex;

            owner.Add(this);
            UpdateDelta();
        }
        #endregion
    }
}
