using System;

namespace ModelGraph.Core
{
    public class EnumRoot : StoreOf<EnumZ>
    {
        #region Constructors  =================================================
        internal EnumRoot(Chef chef)
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
        internal override IdKey IdKey => IdKey.EnumZStore;
        #endregion
    }
}
