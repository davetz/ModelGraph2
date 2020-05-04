
namespace ModelGraph.Core
{
    public class Property_ComputeX_ValueType : PropertyOf<ComputeX, string>
    {
        internal override IdKey IdKey => IdKey.ComputeXValueTypeProperty;
        internal override bool IsReadonly => true;

        internal Property_ComputeX_ValueType(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_ValueType>().GetEnumName(chef, (int)Cast(item).Value.ValType); }
        internal override string GetParentName(Chef chef, Item item) => chef.Get<Relation_Store_ComputeX>().TryGetParent(item, out Store p) ? p.GetSingleNameId(chef) : InvalidItem;
    }
}
