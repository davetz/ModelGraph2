
namespace ModelGraph.Core
{
    public class Property_QueryX_Facet1 : PropertyOf<QueryX, string>
    {
        internal override IdKey ViKey => IdKey.QueryXFacet1Property;

        internal Property_QueryX_Facet1(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_Facet>().GetEnumName(chef, (int)Cast(item).PathParm.Facet1); }
        internal override void SetValue(Item item, string val) { var chef = DataChef; Cast(item).PathParm.Facet1 = (Facet)chef.Get<Enum_Facet>().GetKey(chef, val); }
    }
}
