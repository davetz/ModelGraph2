using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public partial class Chef : ISerializer
    {
        Guid _formatGuid = new Guid("3DB85BFF-F448-465C-996D-367E6284E913");
        Guid _serilizerGuid = new Guid("DE976A9D-0C50-4B4E-9B46-74404A64A703");

        readonly List<(Guid, ISerializer)> _itemSerializers = new List<(Guid, ISerializer)>();
        readonly List<(Guid, ISerializer)> _linkSerializers = new List<(Guid, ISerializer)>(10);

        #region Register  =====================================================
        public void RegisterItemSerializer((Guid, ISerializer) serializer)
        {
            if (_itemSerializers.Count == 0)
                _itemSerializers.Add((_serilizerGuid, this)); //the internal reference serializer should be first

            _itemSerializers.Add(serializer); //item serializers added according to registration order
        }
        public void RegisterLinkSerializer((Guid, ISerializer) serializer)
        {
            _linkSerializers.Add(serializer); //link serializers will be called last
        }
        public void PopulateItemIndex(Dictionary<Item, int> itemIndex)
        {
        }
        #endregion

        #region Serialize/Deserialize  ========================================
        public void Serialize(DataWriter w)
        {
            var serializers = new List<(Guid, ISerializer)>(_itemSerializers);
            serializers.AddRange(_linkSerializers);

            var itemIndex = new Dictionary<Item, int>(GetItemIndexMaxSize());
            foreach (var (_, s) in serializers) { s.PopulateItemIndex(itemIndex); }

            w.WriteGuid(_formatGuid);
            w.WriteInt32(itemIndex.Count);
            foreach (var (g, s) in serializers)
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
            var serializers = new List<(Guid, ISerializer)>(_itemSerializers);
            serializers.AddRange(_linkSerializers);

            var guid = r.ReadGuid();
            if (guid != _formatGuid) throw new Exception("Invalid serializer format");

            var count = r.ReadInt32();
            var items = new Item[count];

            while ((guid = r.ReadGuid()) != _formatGuid)
            {
                var found = false;
                foreach (var (g, s) in serializers)
                {
                    if (g != guid) continue;
                    found = true;
                    s.ReadData(r, items);
                    break;
                }
                if (!found) throw new Exception("Unknown serializer guid reference");
            }
        }
        int GetItemIndexMaxSize()
        {
            var maxSize = 0;
            foreach (var sto in Items)
            {
                if (sto.HasDesendantCount)
                {
                    if (sto.IsInternal || sto.IsExternal) maxSize++;
                    maxSize += sto.GetDesdantCount();
                }
            }
            return maxSize;
        }
        #endregion

        #region ISerializer  ==================================================
        public bool HasData() => true;
        public void ReadData(DataReader r, Item[] items)
        {
            var count = r.ReadInt32();
            if (count < 0 || count > items.Length)
                throw new Exception("Invalid number of guid references");

            var internalItem = GetInternalItems();

            for (int i = 0; i < count; i++)
            {
                var key = r.ReadInt32();
                if (!internalItem.TryGetValue(key, out Item item))
                    throw new Exception("Unkown key reference");

                items[i] = item;
            }
        }
        Dictionary<int, Item> GetInternalItems()
        {
            var internalItem = new Dictionary<int, Item>(100);
            foreach (var sto in Items)
            {
                if (sto.IsInternal || sto.IsExternal) 
                    sto.RegisterInternal(internalItem);
            }
            return internalItem;
        }

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            var referenceItems = new List<Item>(50);
            var index = 0;
            var items = new List<Item>(itemIndex.Keys);
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
