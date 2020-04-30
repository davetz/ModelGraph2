
namespace ModelGraph.Core
{
    public class Property_QueryX_IsReversed : PropertyOf<QueryX, bool>
    {
        internal override IdKey ViKey => IdKey.QueryXIsReversedProperty;

        internal Property_QueryX_IsReversed(PropertyDomain owner)
        {
            Owner = owner;
            Value = new BoolValue(this);

            owner.Add(this);
        }

        internal override bool GetValue(Chef chef, Item item) => Cast(item).IsReversed;
        internal override void SetValue(Chef chef, Item item, bool val) => Cast(item).IsReversed = val;
    }
}
