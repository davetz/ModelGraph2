
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

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_Facet>().GetEnumName(chef, (int)Cast(item).Facet1); }
        internal override void SetValue(Item item, string val) { var chef = DataChef; Cast(item).Facet1 = (Facet)chef.Get<Enum_Facet>().GetKey(chef, val); }
    }
}
