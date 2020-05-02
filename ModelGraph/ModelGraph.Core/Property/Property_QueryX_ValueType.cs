
namespace ModelGraph.Core
{
    public class Property_QueryX_ValueType : PropertyOf<QueryX, string>
    {
        internal override IdKey ViKey => IdKey.ValueXValueTypeProperty;
        internal override bool IsReadonly => true;

        internal Property_QueryX_ValueType(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_ValueType>().GetEnumName(chef, (int)chef.GetValueType(Cast(item))); }
    }
}
