using System;

namespace ModelGraph.Core
{/*
 */
    public class RowX : Item
    {
        #region Constructors  =================================================
        internal RowX(TableX owner, bool autoExpand = false)
        {
            Owner = owner;
            Trait = Trait.RowX;
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion

        #region Properies/Methods  ============================================
        internal TableX TableX => (Owner as TableX);
        #endregion
    }
}
