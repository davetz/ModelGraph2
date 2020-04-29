
namespace ModelGraph.Core
{
    public class Property_Node_BarWidth : PropertyOf<Node, string>
    {
        internal override IdKey ViKey => IdKey.NodeBarWidthProperty;

        internal Property_Node_BarWidth(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetItem<Enum_BarWidth>().GetEnumName(chef, (int)Cast(item).BarWidth);
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).BarWidth = (BarWidth)chef.GetItem<Enum_BarWidth>().GetKey(chef, val);
    }
}
