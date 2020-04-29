using System;

namespace ModelGraph.Core
{
    public class RelationZStore : StoreOf<Relation>
    {
        #region Constructors  =================================================
        internal RelationZStore(Chef owner)
        {
            Owner = owner;
            SetCapacity(10);
            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey ViKey => IdKey.RelationZStore;
        #endregion
    }
}
