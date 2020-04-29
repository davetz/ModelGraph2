
namespace ModelGraph.Core
{
    public class Property_ColumnX_Name : PropertyOf<ColumnX, string>
    {
        internal override IdKey ViKey => IdKey.ColumnNameProperty;

        internal Property_ColumnX_Name(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => Cast(item).Name;
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).Name = val;
    }
}
