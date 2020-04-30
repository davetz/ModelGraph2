using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public interface ISerializer
    {
        int GetSerializerItemCount(Chef chef);
        void PopulateItemIndex(Chef chef, Dictionary<Item, int> itemIndex); //every item serialized or referenced by a serialized item

        bool HasData(Chef chef);
        void ReadData(Chef chef, DataReader r, Item[] items); //array of all items deserialized or referenced by a deserialized item
        void WriteData(Chef chef, DataWriter w, Dictionary<Item, int> itemIndex); //serialize all user defined items and references 
    }
}
