using Windows.UI.Xaml.Controls;

namespace ModelGraph.Core
{
    public class ItemUpdated : ItemChange
    {
        internal Item Item;
        internal Property Property;
        internal string OldValue;
        internal string NewValue;
        internal override IdKey IdKey =>  IdKey.ItemUpdated;

        #region Constructor  ==================================================
        internal ItemUpdated(ChangeSet owner, Item item, Property property, string oldValue, string newValue, string name)
        {
            Owner = owner;
            _name = name;

            Item = item;
            Property = property;
            OldValue = oldValue;
            NewValue = newValue;

            owner.Add(this);
            UpdateDelta();
        }
        #endregion
    }
}
