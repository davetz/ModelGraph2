
namespace ModelGraph.Core
{
    public class Property_Edge_Facet1 : PropertyOf<Edge, string>
    {
        internal override IdKey ViKey => IdKey.EdgeFace1Property;

        internal Property_Edge_Facet1(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetItem<Enum_Facet>().GetEnumName(chef, (int)Cast(item).Facet1);
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).Facet1 = (Facet)chef.GetItem<Enum_Facet>().GetKey(chef, val);
    }
}
