namespace ModelGraph.Core
{
    public class ItemUnLinked : ItemChange
    {
        internal Item Child;
        internal Item Parent;
        internal Relation Relation;
        internal int ChildIndex;
        internal int ParentIndex;
        internal override IdKey ViKey => IdKey.ItemUnlinked;

        #region Constructor  ==================================================
        internal ItemUnLinked(ChangeSet owner, Relation relation, Item parent, Item child, int parentIndex, int childIndex, string name)
        {
            Owner = owner;
            _name = name;

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
