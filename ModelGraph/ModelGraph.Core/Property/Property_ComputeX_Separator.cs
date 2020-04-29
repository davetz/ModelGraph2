
namespace ModelGraph.Core
{
    public class Property_ComputeX_Separator : PropertyOf<ComputeX, string>
    {
        internal override IdKey ViKey => IdKey.ComputeXSeparatorProperty;

        internal Property_ComputeX_Separator(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => Cast(item).Separator;
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).Separator = val;
    }
}
