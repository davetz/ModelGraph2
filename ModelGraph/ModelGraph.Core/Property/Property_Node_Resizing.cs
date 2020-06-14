
namespace ModelGraph.Core
{
    public class Property_Node_Resizing : PropertyOf<Node, string>
    {
        internal override IdKey IdKey => IdKey.NodeResizingProperty;

        internal Property_Node_Resizing(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var root = DataRoot; return root.Get<Enum_Resizing>().GetEnumName(root, (int)Cast(item).Sizing); }
        internal override void SetValue(Item item, string val) { var root = DataRoot; Cast(item).Sizing = (Sizing)root.Get<Enum_Resizing>().GetKey(root, val); }
    }
}
