using System;

namespace ModelGraph.Core
{
    public class StoreOf_PropertyZ : StoreOf<Property>
    {
        #region Constructors  =================================================
        internal StoreOf_PropertyZ(Chef owner)
        {
            Owner = owner;
            SetCapacity(10);
            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.PropertyZStore;
        #endregion
    }
}
