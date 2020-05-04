
namespace ModelGraph.Core
{
    public class Property_ComputeX_Where : PropertyOf<ComputeX, string>
    {
        internal override IdKey IdKey => IdKey.ComputeXWhereProperty;

        internal Property_ComputeX_Where(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var chef = DataChef; return chef.GetWhereProperty(Cast(item)); }
        internal override void SetValue(Item item, string val) { var chef = DataChef; chef.TrySetWhereProperty(Cast(item), val); }
    }
}
