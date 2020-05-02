﻿
namespace ModelGraph.Core
{
    public class Property_Item_Description : PropertyOf<Item, string>
    {
        internal override IdKey ViKey => IdKey.ItemDescriptionProperty;

        internal Property_Item_Description(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) => Description;
        internal override void SetValue(Item item, string val) => Description = val;
    }
}
