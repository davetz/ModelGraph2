
namespace ModelGraph.Core
{
    public class Property_ComputeX_Select : PropertyOf<ComputeX, string>
    {
        internal override IdKey IdKey => IdKey.ComputeXSelectProperty;

        internal Property_ComputeX_Select(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) => DataChef.GetSelectProperty(Cast(item));
        internal override void SetValue(Item item, string val) => DataChef.TrySetSelectProperty(Cast(item), val);
    }
}
