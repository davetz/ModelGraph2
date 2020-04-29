
namespace ModelGraph.Core
{
    public class Property_ColumnX_Summary : PropertyOf<ColumnX, string>
    {
        internal override IdKey ViKey => IdKey.ColumnSummaryProperty;

        internal Property_ColumnX_Summary(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => Cast(item).Summary;
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).Summary = val;
    }
}
