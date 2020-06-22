using System;

namespace ModelGraph.Core
{
    public class EnumRoot : StoreOf<EnumZ>
    {
        #region Constructors  =================================================
        internal EnumRoot(Root root)
        {
            Owner = root;
            SetCapacity(20);
            root.RegisterPrivateItem(new Enum_ValueType(this));
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.EnumZRoot;
        #endregion
    }
}
