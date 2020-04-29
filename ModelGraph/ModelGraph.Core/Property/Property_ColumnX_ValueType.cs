
namespace ModelGraph.Core
{
    public class Property_ColumnX_ValueType : PropertyOf<ColumnX, string>
    {
        internal override IdKey ViKey => IdKey.ColumnValueTypeProperty;

        internal Property_ColumnX_ValueType(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetItem<Enum_ValueType>().GetEnumName(chef, (int)Cast(item).Value.ValType);
        internal override void SetValue(Chef chef, Item item, string val) => chef.SetColumnValueType(Cast(item), chef.GetItem<Enum_ValueType>().GetKey(chef, val));
        internal override string GetParentName(Chef chef, Item item) => chef.GetItem<Relation_Store_ColumnX>().TryGetParent(item, out Store p) ? p.GetSingleNameId(chef) : InvalidItem;
    }
}
