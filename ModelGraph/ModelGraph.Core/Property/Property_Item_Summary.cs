
namespace ModelGraph.Core
{
    public class Property_Item_Summary : PropertyOf<Item, string>
    {
        internal override IdKey IdKey => IdKey.ItemSummaryProperty;

        internal Property_Item_Summary(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) => Summary;
        internal override void SetValue(Item item, string val) => Summary = val;
    }
}
