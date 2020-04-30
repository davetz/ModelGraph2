
namespace ModelGraph.Core
{
    public class Property_QueryX_LineColor : PropertyOf<QueryX, string>
    {
        internal override IdKey ViKey => IdKey.QueryXLineColorProperty;

        internal Property_QueryX_LineColor(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => Cast(item).PathParm.LineColor;
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).PathParm.LineColor = val;
    }
}
