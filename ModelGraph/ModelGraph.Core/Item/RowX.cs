﻿using System;

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
        internal override string GetSingleNameId(Chef chef) => chef.Store_NameProperty.TryGetChild(Owner, out Property p) ? p.Value.GetString(this) : $"#{Index}";
        internal override string GetSummaryId(Chef chef) => chef.Store_SummaryProperty.TryGetChild(Owner, out Property p) ? p.Value.GetString(this) : GetSingleNameId(chef);
        #endregion

        #region Properies/Methods  ============================================
        internal TableX TableX => (Owner as TableX);
        #endregion
    }
}
