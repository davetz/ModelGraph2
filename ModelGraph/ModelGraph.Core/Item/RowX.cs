using System;

namespace ModelGraph.Core
{
    public class RowX : Item
    {
        #region Constructors  =================================================
        internal RowX(TableX owner, bool autoExpand = false)
        {
            Owner = owner;
            OldIdKey = IdKey.RowX;
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey VKey => IdKey.RowX;
        internal override string SingleNameId => GetChef().Store_NameProperty.TryGetChild(Owner, out Property p) ? p.Value.GetString(this) : $"#{Index}";
        internal override string SummaryId => GetChef().Store_SummaryProperty.TryGetChild(Owner, out Property p) ? p.Value.GetString(this) : SingleNameId;
        #endregion

        #region Properies/Methods  ============================================
        internal TableX TableX => (Owner as TableX);
        #endregion
    }
}
