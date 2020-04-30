
namespace ModelGraph.Core
{
    public class Property_QueryX_Where : PropertyOf<QueryX, string>
    {
        internal override IdKey ViKey => IdKey.QueryXWhereProperty;

        internal Property_QueryX_Where(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => Cast(item).WhereString;
        internal override void SetValue(Chef chef, Item item, string val) => chef.TrySetWhereProperty(Cast(item), val);
        internal override string GetParentName(Chef chef, Item item) => chef.GetWhereName(Cast(item));
    }
}
