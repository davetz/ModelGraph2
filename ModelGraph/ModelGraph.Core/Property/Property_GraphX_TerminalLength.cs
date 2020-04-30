
namespace ModelGraph.Core
{
    public class Property_GraphX_TerminalLength : PropertyOf<GraphX, int>
    {
        internal override IdKey ViKey => IdKey.GraphTerminalLengthProperty;

        internal Property_GraphX_TerminalLength(PropertyDomain owner)
        {
            Owner = owner;
            Value = new Int32Value(this);

            owner.Add(this);
        }

        internal override int GetValue(Chef chef, Item item) => Cast(item).TerminalLength;
        internal override void SetValue(Chef chef, Item item, int val) => Cast(item).TerminalLength = (byte)val;
    }
}
