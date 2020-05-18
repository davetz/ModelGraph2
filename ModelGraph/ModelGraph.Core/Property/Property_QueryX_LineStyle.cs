
namespace ModelGraph.Core
{
    public class Property_QueryX_LineStyle : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.QueryXLineStyleProperty;

        internal Property_QueryX_LineStyle(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_LineStyle>().GetEnumName(chef, (int)Cast(item).PathParm.LineStyle); }
        internal override void SetValue(Item item, string val) { var chef = DataChef; Cast(item).PathParm.LineStyle = (LineStyle)chef.Get<Enum_LineStyle>().GetKey(chef, val); }
    }
}
