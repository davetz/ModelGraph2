using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    class RelationXLink : ISerializer
    {
        public Guid GetGuid() => new Guid("61662F08-F43A-44D9-A9BB-9B0126492B8C");

        public void ReadData(DataReader r, Item[] items)
        {
            var index = r.ReadInt32();
            var count = r.ReadInt32();

            if (index < 0 || index >= items.Length) throw new Exception($"Invalid relation index {index}");

            var rel = items[index] as RelationX;
            if (rel is null) throw new Exception("The item is not a relation");

            for (int i = 0; i < count; i++)
            {
                var index1 = r.ReadUInt32();
                var index2 = r.ReadUInt32();
                var len = r.ReadUInt16();

                if (index1 >= items.Length) throw new Exception($"Invalid index1 {index1}");
                if (index2 >= items.Length) throw new Exception($"Invalid index2 {index2}");

                var item1 = items[index1];
                if (item1 is null) throw new Exception("The item1 does not exist");

                var item2 = items[index2];
                if (item2 is null) throw new Exception("The item2 does not exist");

                rel.SetLink(item1, item2, len);
            }
        }

        public void WriteData(DataWriter w)
        {
            throw new NotImplementedException();
        }
    }
}
