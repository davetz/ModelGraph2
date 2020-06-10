﻿
namespace ModelGraph.Core
{
    public class Property_QueryX_Facet2 : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.QueryXFacet2Property;

        internal Property_QueryX_Facet2(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var root = DataChef; return root.Get<Enum_Facet>().GetEnumName(root, (int)Cast(item).PathParm.Facet2); }
        internal override void SetValue(Item item, string val) { var root = DataChef; Cast(item).PathParm.Facet2 = (Facet)root.Get<Enum_Facet>().GetKey(root, val); }
    }
}
