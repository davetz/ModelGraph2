using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class RelationXStore : StoreOf<RelationX>, ISerializer
    {
        static Guid _serializerGuid = new Guid("D950F508-B774-4838-B81A-757EFDC40518");
        static Guid _internalItemGuid = new Guid("BD104B70-CB79-42C3-858D-588B6B868269");
        readonly RelationXLink _linkSerializer;
        internal RelationXStore(Chef owner) : base(owner, Trait.RelationXStore, 30)
        {
            owner.RegisterInternalItem(this, _internalItemGuid);
            owner.RegisterItemSerializer((_serializerGuid, this));
            _linkSerializer = new RelationXLink(owner, this);
        }

        #region ISerializer  ==================================================
        public int GetCount() => Count;

        public void ReadData(DataReader r, Item[] items)
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
        #endregion
    }
}
