
namespace ModelGraph.Core
{
    public class Property_Edge_Facet2 : PropertyOf<Edge, string>
    {
        internal override IdKey IdKey => IdKey.EdgeFace1Property;

        internal Property_Edge_Facet2(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var root = DataRoot; return root.Get<Enum_Facet>().GetEnumValueName(root, (int)Cast(item).Facet2); }
        internal override void SetValue(Item item, string val) { var root = DataRoot; Cast(item).Facet2 = (Facet)root.Get<Enum_Facet>().GetKey(root, val); }
    }
}
