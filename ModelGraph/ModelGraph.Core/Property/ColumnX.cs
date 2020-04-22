using System;

namespace ModelGraph.Core
{
    public class ColumnX : Property
    {
        internal string Name;
        internal string Summary;
        internal string Description;

        #region Constructors  =================================================
        internal ColumnX(StoreOf<ColumnX> owner, bool autoExpand = false)
        {
            Owner = owner;
            IdKey = IdKey.ColumnX;
            if (autoExpand) AutoExpandRight = true;

            Value = Value.Create(ValType.String);

            owner.Add(this);
        }
        #endregion

        #region Property  =====================================================
        internal override bool HasItemName => false;
        internal override string GetItemName(Item key) => null;
        #endregion
    }
}
