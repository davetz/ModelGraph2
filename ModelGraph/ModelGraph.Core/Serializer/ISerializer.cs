using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public interface ISerializer
    {
        bool HasData();
        void ReadData(DataReader r, Item[] items); //array of all items deserialized or referenced by a deserialized item
        void WriteData(DataWriter w, Dictionary<Item, int> itemIndex); //serialize all user defined items and references 
    }
}
