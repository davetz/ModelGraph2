
namespace ModelGraph.Core
{
    public class Property_ColumnX_ValueType : PropertyOf<ColumnX, string>
    {
        internal override IdKey IdKey => IdKey.ColumnValueTypeProperty;

        internal Property_ColumnX_ValueType(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_ValueType>().GetEnumName(chef, (int)Cast(item).Value.ValType); }
        internal override void SetValue(Item item, string val) { var chef = DataChef; chef.SetColumnValueType(Cast(item), chef.Get<Enum_ValueType>().GetKey(chef, val)); }
        internal override string GetParentName(Chef chef, Item item) => chef.Get<Relation_Store_ColumnX>().TryGetParent(item, out Store p) ? p.GetSingleNameId(chef) : InvalidItem;

        internal override int GetIndexValue(Chef chef, Item item) => chef.Get<Enum_ValueType>().GetEnumIndex(chef, (int)Cast(item).Value.ValType);
        internal override string[] GetlListValue(Chef chef, Item item) => chef.Get<Enum_ValueType>().GetEnumNames(chef);
    }
}
