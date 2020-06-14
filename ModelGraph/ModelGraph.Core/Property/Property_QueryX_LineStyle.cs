
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

        internal override string GetValue(Item item) { var root = DataRoot; return root.Get<Enum_LineStyle>().GetEnumName(root, (int)Cast(item).PathParm.LineStyle); }
        internal override void SetValue(Item item, string val) { var root = DataRoot; Cast(item).PathParm.LineStyle = (LineStyle)root.Get<Enum_LineStyle>().GetKey(root, val); }
    }
}
