﻿using System;

namespace ModelGraph.Core
{/*
 */
    public class EnumZ : StoreOf<PairZ>
    {
        #region Constructor  ==================================================
        internal EnumZ(StoreOf<EnumZ> owner, Trait trait)
        {
            Owner = owner;
            Trait = trait;
        }
        #endregion
    }
}
