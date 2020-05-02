﻿
namespace ModelGraph.Core
{
    public class Property_QueryX_Where : PropertyOf<QueryX, string>
    {
        internal override IdKey ViKey => IdKey.QueryXWhereProperty;

        internal Property_QueryX_Where(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) => Cast(item).WhereString;
        internal override void SetValue(Item item, string val) => DataChef.TrySetWhereProperty(Cast(item), val);
        internal override string GetParentName(Chef chef, Item item) => DataChef.GetWhereName(Cast(item));
    }
}