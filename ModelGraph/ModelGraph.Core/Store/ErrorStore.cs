using System;

namespace ModelGraph.Core
{
    public class ErrorStore : StoreOf<Error>
    {
        #region Constructors  =================================================
        internal ErrorStore(Chef owner)
        {
            Owner = owner;
            SetCapacity(20);
            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey VKey => IdKey.ErrorStore;
        #endregion
    }
}
