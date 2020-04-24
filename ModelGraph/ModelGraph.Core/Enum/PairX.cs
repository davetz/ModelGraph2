﻿using System;

namespace ModelGraph.Core
{
    public class PairX : Item
    {
        internal string DisplayValue;
        internal string ActualValue;

        #region Constructors  =================================================
        internal PairX(EnumX owner, bool autoExpand = false)
        {
            Owner = owner;
            OldIdKey = IdKey.PairX;
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        internal EnumX EnumX => Owner as EnumX;
        #endregion

        #region Identity  =====================================================
        internal override IdKey VKey => IdKey.PairX;
        internal override string GetSingleNameId(Chef chef) => DisplayValue;
        #endregion
    }
}
