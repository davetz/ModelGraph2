
namespace ModelGraph.Core
{
    public class Property_Node_Labeling : PropertyOf<Node, string>
    {
        internal override IdKey IdKey => IdKey.NodeLabelingProperty;

        internal Property_Node_Labeling(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var root = DataRoot; return root.Get<Enum_Labeling>().GetEnumName(root, (int)Cast(item).Labeling); }
        internal override void SetValue(Item item, string val) { var root = DataRoot; Cast(item).Labeling = (Labeling)root.Get<Enum_Labeling>().GetKey(root, val); }
    }
}
