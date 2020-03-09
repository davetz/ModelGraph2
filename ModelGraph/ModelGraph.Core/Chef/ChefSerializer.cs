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
        Dictionary<Guid, Item> _internalGuids = new Dictionary<Guid, Item>();
        Dictionary<Item, Guid> _internalItems = new Dictionary<Item, Guid>();

        #region Register  =====================================================
        public void RegisterItemSerializer((Guid, ISerializer) serializer )
        {
            if (_serializers.Count == 0)
                _serializers.Add((_serilizerGuid, this));

            _serializers.Insert(1, serializer);
        }
        public void RegisterLinkSerializer((Guid, ISerializer) serializer)
        {
            _serializers.Add(serializer);
        }
        public void RegisterInternalItem(Item item, Guid guid)
        {
            _internalItems[item] = guid;
            _internalGuids[guid] = item;
        }
        #endregion

        #region Serialize/Deserialize  ========================================
        public void Serialize(DataWriter w)
        {
            var itemIndex = new Dictionary<Item, int>();

            w.WriteGuid(_formatGuid);
            w.WriteInt32(itemIndex.Count);
            foreach (var (g, s) in _serializers)
            {
                if (s.HasData)
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
        public bool HasData => true;
        public void ReadData(DataReader r, Item[] items)
        {
            var count = r.ReadInt32();
            if (count < 0 || count > items.Length)
                throw new Exception("Invalid number of guid references");

            for (int i = 0; i < count; i++)
            {
                var guid = r.ReadGuid();
                if (!_internalGuids.TryGetValue(guid, out Item item))
                    throw new Exception("Unkown guid reference");

                items[i] = item;
            }
        }

        public void RegisterSerializer(Chef chef) { }

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            var referenceItemGuids = new List<(Item, Guid)>();
            var index = 0;
            var items = itemIndex.Keys;
            foreach( var item in items)
            {
                if (_internalItems.TryGetValue(item, out Guid guid))
                {
                    itemIndex[item] = index;
                    referenceItemGuids[index++] = (item, guid);
                }
            }
            var N = referenceItemGuids.Count;

            if (N > 0)
            {
                w.WriteInt32(N);
                foreach ( var (_, guid) in referenceItemGuids)
                { 
                    w.WriteGuid(guid);
                }
            }
            foreach (var item in items)
            {
                if (_internalItems.ContainsKey(item)) continue;
                itemIndex[item] = index++;
            }
        }
        #endregion
    }
}
