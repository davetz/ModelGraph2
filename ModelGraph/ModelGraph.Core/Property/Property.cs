
namespace ModelGraph.Core
{
    public abstract class Property : Item
    {
        internal override bool IsReference => true;
        internal Value Value = Chef.ValuesUnknown;

        internal abstract bool HasItemName { get; }
        internal abstract string GetItemName(Item itm);
    }
}
