using System;

namespace ModelGraph.Core
{
    public class EnumZStore : StoreOf<EnumZ>
    {
        #region Constructors  =================================================
        internal EnumZStore(Chef chef)
        {
            Owner = chef;
            SetCapacity(20);
            AddChildren(chef);

            chef.Add(this);
        }
        #endregion

        #region AddChildren  ==================================================
        void AddChildren(Chef chef)
        {
            chef.RegisterPrivateItem(new Enum_ValueType(this));

        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey ViKey => IdKey.EnumZStore;
        #endregion
    }
}
