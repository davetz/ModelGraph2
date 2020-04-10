using System;

namespace ModelGraph.Core
{
    public class ColumnX : Property
    {
        internal string Name;
        internal string Summary;
        internal string Initial;
        internal string Description;

        #region Constructors  =================================================
        internal ColumnX(StoreOf<ColumnX> owner, bool autoExpand = false)
        {
            Owner = owner;
            Trait = Trait.ColumnX;
            if (autoExpand) AutoExpandRight = true;

            Value = Value.Create(ValType.String);

            owner.Add(this);
        }
        #endregion

        #region Property  =====================================================
        internal override bool HasItemName => false;
        internal override string GetItemName(Item key) => null;

        internal void Initialize(ValType type, string defaultVal, int rowCount)
        {
            Value = Value.Create(type, rowCount, defaultVal);
        }
        #endregion
    }
}
