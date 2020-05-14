
namespace ModelGraph.Core
{
    public abstract class Property : Item
    {
        internal Value Value = Chef.ValuesUnknown;

        internal virtual bool HasParentName => false;
        internal virtual string GetParentName(Chef chef, Item itm) => default;

        internal virtual bool IsReadonly => false;
        internal virtual bool IsMultiline => false;

        internal virtual string[] GetlListValue(Chef chef) => new string[0];
        internal virtual int GetIndexValue(Item item) => 0;
        internal virtual void SetIndexValue(Item item, int val) { }

    }
}
