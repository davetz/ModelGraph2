
namespace ModelGraph.Core
{
    public class Property_Node_Labeling : PropertyOf<Node, string>
    {
        internal override IdKey ViKey => IdKey.NodeLabelingProperty;

        internal Property_Node_Labeling(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetItem<Enum_Labeling>().GetEnumName(chef, (int)Cast(item).Labeling);
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).Labeling = (Labeling)chef.GetItem<Enum_Labeling>().GetKey(chef, val);
    }
}
