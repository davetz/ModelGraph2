using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    class RelationXLink : ISerializer
    {
        static Guid _serializerGuid => new Guid("61662F08-F43A-44D9-A9BB-9B0126492B8C");
        readonly RelationXStore _relStore;
        internal RelationXLink(Chef chef, RelationXStore relStore)
        {
            _relStore = relStore;
            chef.RegisterLinkSerializer((_serializerGuid, this));
        }
        public bool HasData => GetHasData();
        bool GetHasData()
        {
            foreach (var itm in _relStore.Items)
            {
                if (itm is RelationX rx && rx.GetLinksCount() > 0) return true;
            }
            return false;
        }

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            foreach (var itm in _relStore.Items)
            {
                if (itm is RelationX rx) 
                {
                    var count = rx.GetLinksCount();
                    if (count > 0)
                    {
                        ushort len = 0;
                        rx.GetLinks(out List<Item> parents, out List<Item> children);

                        int N = count;
                        for (int j = 0; j < count; j++)
                        {
                            var child = children[j];
                            var parent = parents[j];
                            if (itemIndex.ContainsKey(child) && itemIndex.ContainsKey(parent)) continue;

                            // null out this is link, it should not be serialized
                            children[j] = null;
                            parents[j] = null;
                            N -= 1;
                        }
                        if (N == 0) continue;

                        w.WriteByte((byte)Mark.RelationLinkBegin); // type index
                        w.WriteInt32(itemIndex[rx]);
                        w.WriteInt32(N);

                        for (int j = 0; j < count; j++)
                        {
                            var child = children[j];
                            var parent = parents[j];
                            if (child == null || parent == null) continue;

                            if (itm != parent)
                            {
                                len = 1;
                                itm = parent;
                                for (int k = j + 1; k < count; k++)
                                {
                                    if (parents[k] != itm) break;
                                    if (len < ushort.MaxValue) len += 1;
                                }
                            }

                            w.WriteInt32(itemIndex[parent]);
                            w.WriteInt32(itemIndex[child]);
                            w.WriteUInt16(len);
                            len = 0;
                        }
                    }
                }

            }
        }

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
    }
}
