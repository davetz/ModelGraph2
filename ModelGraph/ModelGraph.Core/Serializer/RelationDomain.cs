using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class RelationDomain : InternalDomainOf<Relation>, IRelationStore
    {
        internal RelationDomain(Chef owner) : base(owner, IdKey.RelationDomain, 30)
        {
            new RelationLink(owner, this);
        }

        #region CreateRelations  ==============================================
        private void CreateRelations()
        {

        }
        #endregion

        #region GetRelationArray  =============================================
        public Relation[] GetRelationArray()
        {
            var relationArray = new Relation[Count];
            for (int i = 0; i < Count; i++)
            {
                relationArray[i] = Items[i];
            }
            return relationArray;
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey ViKey => IdKey.RelationDomain;
        public override string GetParentNameId(Chef chef) => GetKindId(chef);
        #endregion
    }
}
