using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class RelationStore : InternalStore<Relation>
    {
        internal RelationStore(Chef owner) : base(owner, Trait.RelationStore, 30)
        {
            new RelationLink(owner, this);
        }
    }
}
