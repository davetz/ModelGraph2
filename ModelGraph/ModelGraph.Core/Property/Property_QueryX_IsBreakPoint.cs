
namespace ModelGraph.Core
{
    public class Property_QueryX_IsBreakPoint : PropertyOf<QueryX, bool>
    {
        internal override IdKey ViKey => IdKey.QueryXIsBreakPointProperty;

        internal Property_QueryX_IsBreakPoint(PropertyDomain owner)
        {
            Owner = owner;
            Value = new BoolValue(this);

            owner.Add(this);
        }

        internal override bool GetValue(Chef chef, Item item) => Cast(item).IsBreakPoint;
        internal override void SetValue(Chef chef, Item item, bool val) => Cast(item).IsBreakPoint = val;
    }
}
