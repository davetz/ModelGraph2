using System.Collections.Generic;

namespace ModelGraph.Core
{
    public interface IDomain
    {
        void RegisterInternal(Dictionary<int, Item> internalItem);
        int GetSerializerItemCount();
        void PopulateItemIndex(Dictionary<Item, int> itemIndex); //every item serialized or referenced by a serialized item
    }
}
