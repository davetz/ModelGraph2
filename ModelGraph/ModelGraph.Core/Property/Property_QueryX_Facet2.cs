
namespace ModelGraph.Core
{
    public class Property_QueryX_Facet2 : PropertyOf<QueryX, string>
    {
        internal override IdKey ViKey => IdKey.QueryXFacet2Property;

        internal Property_QueryX_Facet2(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetItem<Enum_Facet>().GetEnumName(chef, (int)Cast(item).PathParm.Facet2);
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).PathParm.Facet2 = (Facet)chef.GetItem<Enum_Facet>().GetKey(chef, val);
    }
}
