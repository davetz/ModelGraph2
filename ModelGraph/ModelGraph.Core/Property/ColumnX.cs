using System;

namespace ModelGraph.Core
{
    public class ColumnX : Property
    {
        override internal string Name { get; set; }
        override internal string Summary { get; set; }
        override internal string Description { get; set; }

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
        #endregion
    }
}
