using System;

namespace ModelGraph.Core
{
    public class ItemLinked : ItemChange
    {
        internal Item Child;
        internal Item Parent;
        internal Relation Relation;
        internal int ParentIndex;
        internal int ChildIndex;
        internal override IdKey IdKey =>  IdKey.ItemLinked;

        #region Constructor  ==================================================
        internal ItemLinked(ChangeSet owner, Relation relation, Item parent, Item child, int parentIndex, int childIndex, string name)
        {
            Owner = owner;
            _name = name;

            Child = child;
            Parent = parent;
            Relation = relation;
            ChildIndex = parentIndex;
            ParentIndex = parentIndex;

            owner.Add(this);
        }
        #endregion
    }
}
