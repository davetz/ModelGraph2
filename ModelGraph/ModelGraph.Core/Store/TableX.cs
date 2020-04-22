using System;

namespace ModelGraph.Core
{
    public class TableX : StoreOf<RowX>
    {
        internal string Name;
        internal string Summary;
        internal string Description;

        #region Constructors  =================================================
        internal TableX(StoreOf<TableX> owner, bool autoExpand = false)
        {
            Owner = owner;
            IdKey = IdKey.TableX;
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion
    }
}
