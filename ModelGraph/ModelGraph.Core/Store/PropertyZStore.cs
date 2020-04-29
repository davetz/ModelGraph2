using System;

namespace ModelGraph.Core
{
    public class PropertyZStore : StoreOf<Property>
    {
        #region Constructors  =================================================
        internal PropertyZStore(Chef owner)
        {
            Owner = owner;
            SetCapacity(10);
            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey ViKey => IdKey.PropertyZStore;
        #endregion
    }
}
