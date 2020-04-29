
namespace ModelGraph.Core
{
    public class Property_PairX_Value : PropertyOf<PairX, string>
    {
        internal override IdKey ViKey => IdKey.EnumValueProperty;

        internal Property_PairX_Value(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => Cast(item).ActualValue;
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).ActualValue = val;
    }
}
