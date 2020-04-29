
namespace ModelGraph.Core
{
    public class Property_ComputeX_Where : PropertyOf<ComputeX, string>
    {
        internal override IdKey ViKey => IdKey.ComputeXWhereProperty;

        internal Property_ComputeX_Where(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetWhereProperty(Cast(item));
        internal override void SetValue(Chef chef, Item item, string val) => chef.TrySetWhereProperty(Cast(item), val);
    }
}
