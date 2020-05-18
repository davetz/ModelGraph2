using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    class RelationXLink : LinkSerializer, ISerializer
    {
        static Guid _serializerGuid => new Guid("61662F08-F43A-44D9-A9BB-9B0126492B8C");

        internal RelationXLink(Chef chef, RelationXRoot relationStore) : base(relationStore)
        {
            chef.RegisterLinkSerializer((_serializerGuid, this));
        }

        public void PopulateItemIndex(Dictionary<Item, int> itemIndex) { }

        public void RegisterInternal(Dictionary<int, Item> internalItem) { }
        public int GetSerializerItemCount() => 0;
    }
}
