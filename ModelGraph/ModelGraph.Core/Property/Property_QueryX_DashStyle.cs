
namespace ModelGraph.Core
{
    public class Property_QueryX_DashStyle : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.QueryXDashStyleProperty;

        internal Property_QueryX_DashStyle(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_DashStyle>().GetEnumName(chef, (int)Cast(item).PathParm.DashStyle); }
        internal override void SetValue(Item item, string val) { var chef = DataChef; Cast(item).PathParm.DashStyle = (DashStyle)chef.Get<Enum_DashStyle>().GetKey(chef, val); }
    }
}
