using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public abstract class XStoreOf<T> : StoreOf<T> where T : Item, ISerializer 
    {
        public abstract Guid GetGuid();
        public abstract void ReadData(DataReader r, Item[] items);
        public abstract void WriteData(DataWriter w);
    }
}
