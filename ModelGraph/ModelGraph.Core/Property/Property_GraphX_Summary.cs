
namespace ModelGraph.Core
{
    public class Property_GraphX_Summary : PropertyOf<GraphX, string>
    {
        internal override IdKey ViKey => IdKey.GraphSummaryProperty;

        internal Property_GraphX_Summary(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => Cast(item).Summary;
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).Summary = val;
    }
}
