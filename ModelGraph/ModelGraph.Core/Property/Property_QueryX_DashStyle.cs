
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

        internal override string GetValue(Item item) { var root = DataRoot; return root.Get<Enum_DashStyle>().GetEnumValueName(root, (int)Cast(item).PathParm.DashStyle); }
        internal override void SetValue(Item item, string val) { var root = DataRoot; Cast(item).PathParm.DashStyle = (DashStyle)root.Get<Enum_DashStyle>().GetKey(root, val); }
    }
}
