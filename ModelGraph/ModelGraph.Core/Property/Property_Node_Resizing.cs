
namespace ModelGraph.Core
{
    public class Property_Node_Resizing : PropertyOf<Node, string>
    {
        internal override IdKey ViKey => IdKey.NodeResizingProperty;

        internal Property_Node_Resizing(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetItem<Enum_Resizing>().GetEnumName(chef, (int)Cast(item).Sizing);
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).Sizing = (Sizing)chef.GetItem<Enum_Resizing>().GetKey(chef, val);
    }
}
