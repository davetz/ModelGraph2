
namespace ModelGraph.Core
{
    public class Property_Node_Aspect : PropertyOf<Node, string>
    {
        internal override IdKey IdKey => IdKey.NodeOrientationProperty;

        internal Property_Node_Aspect(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var root = DataChef; return root.Get<Enum_Aspect>().GetEnumName(root, (int)Cast(item).Aspect); }
        internal override void SetValue(Item item, string val) { var root = DataChef; Cast(item).Aspect = (Aspect)root.Get<Enum_Aspect>().GetKey(root, val); }
    }
}
