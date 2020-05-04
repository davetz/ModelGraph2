namespace ModelGraph.Core
{
    public class ItemMoved : ItemChange
    {
        internal Item Item;
        internal int Index1; // the initial location before the move
        internal int Index2; // the ending location after the move
        internal override IdKey IdKey => IdKey.ItemMoved;

        #region Constructor  ==================================================
        internal ItemMoved(ChangeSet owner, Item item, int index1, int index2, string name)
        {
            Owner = owner;
            _name = name;

            Item = item;
            Index1 = index1;
            Index2 = index2;

            owner.Add(this);
            UpdateDelta();
        }
        #endregion
    }
}
