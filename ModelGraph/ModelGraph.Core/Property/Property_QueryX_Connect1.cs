
namespace ModelGraph.Core
{
    public class Property_QueryX_Connect1 : PropertyOf<QueryX, string>
    {
        internal override IdKey IdKey => IdKey.QueryXConnect1Property;

        internal Property_QueryX_Connect1(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) => DataRoot.GetTargetString(Cast(item).PathParm.Target1);
        internal override void SetValue(Item item, string val) => Cast(item).PathParm.Target1 = DataRoot.GetTargetValue(val);
    }
}
