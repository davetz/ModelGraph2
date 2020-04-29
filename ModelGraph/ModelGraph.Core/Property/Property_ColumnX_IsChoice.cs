
namespace ModelGraph.Core
{
    public class Property_ColumnX_IsChoice : PropertyOf<ColumnX, bool>
    {
        internal override IdKey ViKey => IdKey.ColumnIsChoiceProperty;

        internal Property_ColumnX_IsChoice(PropertyDomain owner)
        {
            Owner = owner;
            Value = new BoolValue(this);

            owner.Add(this);
        }

        internal override bool GetValue(Chef chef, Item item) => Cast(item).IsChoice;
        internal override void SetValue(Chef chef, Item item, bool val) => Cast(item).IsChoice = val;
    }
}
