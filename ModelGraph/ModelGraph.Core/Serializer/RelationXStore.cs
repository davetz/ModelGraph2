using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class RelationXStore : StoreOf<RelationX>, ISerializer
    {
        public bool HasData => throw new NotImplementedException();

        public Guid GetGuid()
        {
            throw new NotImplementedException();
        }

        public void ReadData(DataReader r, Item[] items)
        {
            throw new NotImplementedException();
        }

        public void RegisterSerializer(Chef chef)
        {
            throw new NotImplementedException();
        }

        public void WriteData(DataWriter w)
        {
            throw new NotImplementedException();
        }

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            throw new NotImplementedException();
        }
    }
}
