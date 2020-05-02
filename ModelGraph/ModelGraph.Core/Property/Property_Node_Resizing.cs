
namespace ModelGraph.Core
{
    public class Property_Node_Resizing : PropertyOf<Node, string>
    {
        internal override IdKey ViKey => IdKey.NodeResizingProperty;

        internal Property_Node_Resizing(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_Resizing>().GetEnumName(chef, (int)Cast(item).Sizing); }
        internal override void SetValue(Item item, string val) { var chef = DataChef; Cast(item).Sizing = (Sizing)chef.Get<Enum_Resizing>().GetKey(chef, val); }
    }
}
