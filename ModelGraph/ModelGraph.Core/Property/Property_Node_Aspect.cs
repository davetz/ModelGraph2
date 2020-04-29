
namespace ModelGraph.Core
{
    public class Property_Node_Aspect : PropertyOf<Node, string>
    {
        internal override IdKey ViKey => IdKey.NodeOrientationProperty;

        internal Property_Node_Aspect(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetItem<Enum_AspectType>().GetEnumName(chef, (int)Cast(item).Aspect);
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).Aspect = (Aspect)chef.GetItem<Enum_AspectType>().GetKey(chef, val);
    }
}
