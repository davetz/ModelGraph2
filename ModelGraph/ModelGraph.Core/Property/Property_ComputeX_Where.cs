
namespace ModelGraph.Core
{
    public class Property_ComputeX_Where : PropertyOf<ComputeX, string>
    {
        internal override IdKey IdKey => IdKey.ComputeXWhereProperty;

        internal Property_ComputeX_Where(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var root = DataChef; return root.GetWhereProperty(Cast(item)); }
        internal override void SetValue(Item item, string val) { var root = DataChef; root.TrySetWhereProperty(Cast(item), val); }
    }
}
