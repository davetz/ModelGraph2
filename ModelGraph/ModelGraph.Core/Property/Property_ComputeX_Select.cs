
namespace ModelGraph.Core
{
    public class Property_ComputeX_Select : PropertyOf<ComputeX, string>
    {
        internal override IdKey ViKey => IdKey.ComputeXSelectProperty;

        internal Property_ComputeX_Select(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetSelectProperty(Cast(item));
        internal override void SetValue(Chef chef, Item item, string val) => chef.TrySetSelectProperty(Cast(item), val);
    }
}
