
namespace ModelGraph.Core
{
    public class Property_GraphX_SymbolSize : PropertyOf<GraphX, int>
    {
        internal override IdKey ViKey => IdKey.GraphSymbolSizeProperty;

        internal Property_GraphX_SymbolSize(PropertyDomain owner)
        {
            Owner = owner;
            Value = new Int32Value(this);

            owner.Add(this);
        }

        internal override int GetValue(Chef chef, Item item) => Cast(item).SymbolSize;
        internal override void SetValue(Chef chef, Item item, int val) => Cast(item).SymbolSize = (byte)val;
    }
}
