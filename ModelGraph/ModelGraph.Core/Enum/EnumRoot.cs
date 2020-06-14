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
            AddChildren(root);

            root.Add(this);
        }
        #endregion

        #region AddChildren  ==================================================
        void AddChildren(Root root)
        {
            root.RegisterPrivateItem(new Enum_ValueType(this));
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.EnumZRoot;
        #endregion
    }
}
