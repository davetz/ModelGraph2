
namespace ModelGraph.Core
{
    public class Property_QueryX_ExclusiveKey : PropertyOf<QueryX, byte>
    {
        internal override IdKey ViKey => IdKey.QueryXExclusiveKeyProperty;

        internal Property_QueryX_ExclusiveKey(PropertyDomain owner)
        {
            Owner = owner;
            Value = new ByteValue(this);

            owner.Add(this);
        }

        internal override byte GetValue(Chef chef, Item item) => Cast(item).ExclusiveKey;
        internal override void SetValue(Chef chef, Item item, byte val) => Cast(item).ExclusiveKey = val;
    }
}
