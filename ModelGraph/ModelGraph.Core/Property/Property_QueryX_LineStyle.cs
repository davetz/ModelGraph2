
namespace ModelGraph.Core
{
    public class Property_QueryX_LineStyle : PropertyOf<QueryX, string>
    {
        internal override IdKey ViKey => IdKey.QueryXLineStyleProperty;

        internal Property_QueryX_LineStyle(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetItem<Enum_LineStyle>().GetEnumName(chef, (int)Cast(item).PathParm.LineStyle);
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).PathParm.LineStyle = (LineStyle)chef.GetItem<Enum_LineStyle>().GetKey(chef, val);
    }
}
