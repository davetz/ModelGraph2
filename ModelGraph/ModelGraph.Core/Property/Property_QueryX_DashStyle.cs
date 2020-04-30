
namespace ModelGraph.Core
{
    public class Property_QueryX_DashStyle : PropertyOf<QueryX, string>
    {
        internal override IdKey ViKey => IdKey.QueryXDashStyleProperty;

        internal Property_QueryX_DashStyle(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetItem<Enum_DashStyle>().GetEnumName(chef, (int)Cast(item).PathParm.DashStyle);
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).PathParm.DashStyle = (DashStyle)chef.GetItem<Enum_DashStyle>().GetKey(chef, val);
    }
}
