using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public partial class Chef : ISerializer
    {
        Guid _formatGuid = new Guid("3DB85BFF-F448-465C-996D-367E6284E913");
        Guid _serilizerGuid = new Guid("DE976A9D-0C50-4B4E-9B46-74404A64A703");

        List<(Guid, ISerializer)> _serializers = new List<(Guid, ISerializer)>();
        Dictionary<int, Item> _internalItems = new Dictionary<int, Item>();

        #region Register  =====================================================
        public void RegisterSerializer((Guid, ISerializer) serializer, bool isLinkSerializer = false )
        {
            if (_serializers.Count == 0)
                _serializers.Add((_serilizerGuid, this)); //the internal reference serializer should be first

            if (isLinkSerializer)
                _serializers.Add(serializer); //link serializers should be called last
            else
            _serializers.Insert(1, serializer); //item serializers are in the middle
        }
        public void RegisterInernalItem(Item item)
        {
            _internalItems.Add(item.ItemKey, item);
        }
        public void PopulateItemIndex(Dictionary<Item, int> itemIndex)
        {
        }
        #endregion

        #region Serialize/Deserialize  ========================================
        public void Serialize(DataWriter w)
        {
            var itemIndex = new Dictionary<Item, int>(GetSerializerCount());
            foreach (var (_, s) in _serializers) { s.PopulateItemIndex(itemIndex); }

            w.WriteGuid(_formatGuid);
            w.WriteInt32(itemIndex.Count);
            foreach (var (g, s) in _serializers)
            {
                if (s.HasData())
                {
                    w.WriteGuid(g);
                    s.WriteData(w, itemIndex);
                }
            }
            w.WriteGuid(_formatGuid);
        }
        public void Deserialize(DataReader r)
        {
            var guid = r.ReadGuid();
            if (guid != _formatGuid) throw new Exception("Invalid serializer format");

            var count = r.ReadInt32();
            var items = new Item[count];

            while ((guid = r.ReadGuid()) != _formatGuid)
            {
                var found = false;
                foreach (var (g, s) in _serializers)
                {
                    if (g != guid) continue;
                    found = true;
                    s.ReadData(r, items);
                }
                if (!found) throw new Exception("Unknown guid reference");
            }
        }
        #endregion

        #region ISerializer  ==================================================
        public bool HasData() => true;
        public void ReadData(DataReader r, Item[] items)
        {
            var count = r.ReadInt32();
            if (count < 0 || count > items.Length)
                throw new Exception("Invalid number of guid references");

            for (int i = 0; i < count; i++)
            {
                var key = r.ReadInt32();
                if (!_internalItems.TryGetValue(key, out Item item))
                    throw new Exception("Unkown key reference");

                items[i] = item;
            }
        }

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            var referenceItems = new List<Item>(50);
            var index = 0;
            var items = itemIndex.Keys;
            foreach (var item in items)
            {
                if (item.IsExternal) continue;

                referenceItems.Add(item);
                itemIndex[item] = index++;
            }

            w.WriteInt32(index);
            foreach (var item in referenceItems)
            {
                w.WriteInt32(item.ItemKey); //referenced internal item
            }

            foreach (var item in items)
            {
                if (item.IsExternal) itemIndex[item] = index++;
            }
        }
        #endregion
    }
}
