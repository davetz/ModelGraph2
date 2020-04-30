
namespace ModelGraph.Core
{
    public class Property_GraphX_Name : PropertyOf<GraphX, string>
    {
        internal override IdKey ViKey => IdKey.GraphNameProperty;

        internal Property_GraphX_Name(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => Cast(item).Name;
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).Name = val;
    }
}
