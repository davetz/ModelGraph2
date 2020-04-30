
namespace ModelGraph.Core
{
    public class Property_GraphX_TerminalStretch : PropertyOf<GraphX, int>
    {
        internal override IdKey ViKey => IdKey.GraphTerminalStretchProperty;

        internal Property_GraphX_TerminalStretch(PropertyDomain owner)
        {
            Owner = owner;
            Value = new Int32Value(this);

            owner.Add(this);
        }

        internal override int GetValue(Chef chef, Item item) => Cast(item).TerminalSkew;
        internal override void SetValue(Chef chef, Item item, int val) => Cast(item).TerminalSkew = (byte)val;
    }
}
