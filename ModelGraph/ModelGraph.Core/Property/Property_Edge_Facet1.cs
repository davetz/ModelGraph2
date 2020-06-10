
namespace ModelGraph.Core
{
    public class Property_Edge_Facet1 : PropertyOf<Edge, string>
    {
        internal override IdKey IdKey => IdKey.EdgeFace1Property;

        internal Property_Edge_Facet1(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var root = DataChef; return root.Get<Enum_Facet>().GetEnumName(root, (int)Cast(item).Facet1); }
        internal override void SetValue(Item item, string val) { var root = DataChef; Cast(item).Facet1 = (Facet)root.Get<Enum_Facet>().GetKey(root, val); }
    }
}
