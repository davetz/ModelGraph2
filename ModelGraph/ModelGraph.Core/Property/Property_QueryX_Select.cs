
namespace ModelGraph.Core
{
    public class Property_QueryX_Select : PropertyOf<QueryX, string>
    {
        internal override IdKey ViKey => IdKey.ValueXSelectProperty;

        internal Property_QueryX_Select(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => Cast(item).SelectString;
        internal override void SetValue(Chef chef, Item item, string val) => chef.TrySetSelectProperty(Cast(item), val);
        internal override string GetParentName(Chef chef, Item item) => chef.GetSelectName(Cast(item));
    }
}
