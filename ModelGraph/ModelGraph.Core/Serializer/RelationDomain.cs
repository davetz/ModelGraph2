using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class RelationDomain : InternalDomainOf<Relation>, IRelationStore
    {
        internal RelationDomain(Chef owner) : base(owner, IdKey.RelationStore, 30)
        {
            new RelationLink(owner, this);
        }

        public Relation[] GetRelationArray()
        {
            var relationArray = new Relation[Count];
            for (int i = 0; i < Count; i++)
            {
                relationArray[i] = Items[i];
            }
            return relationArray;
        }

        #region Identity  =====================================================
        internal override IdKey VKey => IdKey.RelationStore;
        internal override string ParentNameId => KindId;
        #endregion
    }
}
