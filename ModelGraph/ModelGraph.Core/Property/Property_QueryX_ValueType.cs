
namespace ModelGraph.Core
{
    public class Property_QueryX_ValueType : PropertyOf<QueryX, string>
    {
        internal override IdKey ViKey => IdKey.ValueXValueTypeProperty;

        internal Property_QueryX_ValueType(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetItem<Enum_ValueType>().GetEnumName(chef, (int)chef.GetValueType(Cast(item)));
    }
}
