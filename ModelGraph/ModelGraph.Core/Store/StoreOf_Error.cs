using System;

namespace ModelGraph.Core
{
    public class StoreOf_Error : StoreOf<Error>
    {
        #region Constructors  =================================================
        internal StoreOf_Error(Chef owner)
        {
            Owner = owner;
            SetCapacity(20);
            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.ErrorStore;
        #endregion
    }
}
