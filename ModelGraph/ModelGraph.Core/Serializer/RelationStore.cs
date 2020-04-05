using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class RelationStore : InternalStoreOf<Relation>, IRelationStore
    {
        internal RelationStore(Chef owner) : base(owner, Trait.RelationStore, 30)
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
    }
}
