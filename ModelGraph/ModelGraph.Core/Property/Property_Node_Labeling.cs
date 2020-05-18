
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

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_Labeling>().GetEnumName(chef, (int)Cast(item).Labeling); }
        internal override void SetValue(Item item, string val) { var chef = DataChef; Cast(item).Labeling = (Labeling)chef.Get<Enum_Labeling>().GetKey(chef, val); }
    }
}
