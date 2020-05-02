
namespace ModelGraph.Core
{
    public class Property_Node_Aspect : PropertyOf<Node, string>
    {
        internal override IdKey ViKey => IdKey.NodeOrientationProperty;

        internal Property_Node_Aspect(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_Aspect>().GetEnumName(chef, (int)Cast(item).Aspect); }
        internal override void SetValue(Item item, string val) { var chef = DataChef; Cast(item).Aspect = (Aspect)chef.Get<Enum_Aspect>().GetKey(chef, val); }
    }
}
