
namespace ModelGraph.Core
{
    public class Property_Node_BarWidth : PropertyOf<Node, string>
    {
        internal override IdKey IdKey => IdKey.NodeBarWidthProperty;

        internal Property_Node_BarWidth(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_BarWidth>().GetEnumName(chef, (int)Cast(item).BarWidth); }
        internal override void SetValue(Item item, string val) { var chef = DataChef; Cast(item).BarWidth = (BarWidth)chef.Get<Enum_BarWidth>().GetKey(chef, val); }
    }
}
