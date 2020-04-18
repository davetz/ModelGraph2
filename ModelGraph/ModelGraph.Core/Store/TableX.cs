using System;

namespace ModelGraph.Core
{
    public class TableX : StoreOf<RowX>
    {
        internal override bool IsExternal => true;
        internal string Name;
        internal string Summary;
        internal string Description;

        #region Constructors  =================================================
        internal TableX(StoreOf<TableX> owner, bool autoExpand = false)
        {
            Owner = owner;
            Trait = Trait.TableX;
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion
    }
}
