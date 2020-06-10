using System;

namespace ModelGraph.Core
{
    public class ErrorRoot : StoreOf<Error>
    {
        #region Constructors  =================================================
        internal ErrorRoot(Root owner)
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
