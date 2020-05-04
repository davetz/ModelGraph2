﻿
namespace ModelGraph.Core
{
    public class Property_QueryX_Relation : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.QueryXRelationProperty;
        internal override bool IsReadonly => true;

        internal Property_QueryX_Relation(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) => DataChef.GetQueryXRelationName(Cast(item));
    }
}
