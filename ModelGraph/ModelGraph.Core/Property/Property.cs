
namespace ModelGraph.Core
{
    public abstract class Property : Item
    {
        internal Value Value = Chef.ValuesUnknown;

        internal virtual bool HasParentName => false;
        internal virtual string GetParentName(Chef chef, Item itm) => default;

        internal virtual bool IsReadonly => false;
        internal virtual bool IsMultiline => false;

    }
}
