
namespace ModelGraph.Core
{
    public class Property_QueryX_Connect2 : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.QueryXConnect2Property;

        internal Property_QueryX_Connect2(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) => DataChef.GetTargetString(Cast(item).PathParm.Target2);
        internal override void SetValue(Item item, string val) => Cast(item).PathParm.Target2 = DataChef.GetTargetValue(val);
    }
}
