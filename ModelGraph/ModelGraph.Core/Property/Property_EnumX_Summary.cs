
namespace ModelGraph.Core
{
    public class Property_EnumX_Summary : PropertyOf<EnumX, string>
    {
        internal override IdKey ViKey => IdKey.EnumSummaryProperty;

        internal Property_EnumX_Summary(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => Cast(item).Summary;
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).Summary = val;
    }
}
