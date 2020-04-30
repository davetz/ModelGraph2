
namespace ModelGraph.Core
{
    public class Property_QueryX_Connect2 : PropertyOf<QueryX, string>
    {
        internal override IdKey ViKey => IdKey.QueryXConnect2Property;

        internal Property_QueryX_Connect2(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetTargetString(Cast(item).PathParm.Target2);
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).PathParm.Target2 = chef.GetTargetValue(val);
    }
}
