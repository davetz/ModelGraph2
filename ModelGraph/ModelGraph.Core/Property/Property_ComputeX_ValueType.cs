
namespace ModelGraph.Core
{
    public class Property_ComputeX_ValueType : PropertyOf<ComputeX, string>
    {
        internal override IdKey ViKey => IdKey.ComputeXValueTypeProperty;

        internal Property_ComputeX_ValueType(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetItem<Enum_ValueType>().GetEnumName(chef, (int)Cast(item).Value.ValType);
        internal override string GetParentName(Chef chef, Item item) => chef.GetItem<Relation_Store_ComputeX>().TryGetParent(item, out Store p) ? p.GetSingleNameId(chef) : InvalidItem;
    }
}
