using System;

namespace ModelGraph.Core
{
    public class StoreOf_RelationZ : StoreOf<Relation>
    {
        #region Constructors  =================================================
        internal StoreOf_RelationZ(Chef owner)
        {
            Owner = owner;
            SetCapacity(10);
            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.RelationZStore;
        #endregion
    }
}
