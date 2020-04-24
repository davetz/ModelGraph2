using System;

namespace ModelGraph.Core
{
    public class EnumZStore : StoreOf<EnumZ>
    {
        #region Constructors  =================================================
        internal EnumZStore(Chef owner)
        {
            Owner = owner;
            SetCapacity(20);
            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey VKey => IdKey.EnumZStore;
        #endregion
    }
}
