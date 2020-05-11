
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
        private EnumZ GetEnum(Chef chef) => chef.Get<Enum_ValueType>();
        private string GetEnumName(Chef chef, int val) => GetEnum(chef).GetEnumName(chef, val);
        private int GetEnumKey(Chef chef, string val) => GetEnum(chef).GetKey(chef, val);
        private int GetEnumIndex(Chef chef, int key) => GetEnum(chef).GetEnumIndex(chef, key);
        private string[] GetEnumNames(Chef chef) => GetEnum(chef).GetEnumNames(chef);


        internal override string GetValue(Item item) => GetEnumName(DataChef, (int)Cast(item).Value.ValType);
        internal override void SetValue(Item item, string val) { var chef = DataChef; chef.SetColumnValueType(Cast(item), GetEnumKey(chef, val)); }
        internal override string GetParentName(Chef chef, Item item) => chef.Get<Relation_Store_ColumnX>().TryGetParent(item, out Store p) ? p.GetSingleNameId(chef) : InvalidItem;

        internal override int GetIndexValue(Chef chef, Item item) => GetEnumIndex(chef, (int)Cast(item).Value.ValType);
        internal override string[] GetlListValue(Chef chef) => GetEnumNames(chef);
    }
}
