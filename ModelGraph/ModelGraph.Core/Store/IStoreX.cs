using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public interface IStoreX
    {
        void ReadData(DataReader r, Item[] items);
        void WriteData(DataWriter w, Dictionary<Item, int> itemIndex);
    }
}
