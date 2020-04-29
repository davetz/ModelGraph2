
namespace ModelGraph.Core
{
    public class Property_EnumX_Name : PropertyOf<EnumX, string>
    {
        internal override IdKey ViKey => IdKey.EnumNameProperty;

        internal Property_EnumX_Name(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => Cast(item).Name;
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).Name = val;
    }
}
