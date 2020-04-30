
namespace ModelGraph.Core
{
    public class Property_QueryX_Connect1 : PropertyOf<QueryX, string>
    {
        internal override IdKey ViKey => IdKey.QueryXConnect1Property;

        internal Property_QueryX_Connect1(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetTargetString(Cast(item).PathParm.Target1);
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).PathParm.Target1 = chef.GetTargetValue(val);
    }
}
