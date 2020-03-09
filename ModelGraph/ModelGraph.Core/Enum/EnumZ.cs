using System;

namespace ModelGraph.Core
{/*
 */
    public class EnumZ : StoreOfOld<PairZ>
    {
        #region Constructor  ==================================================
        internal EnumZ(StoreOfOld<EnumZ> owner, Trait trait)
        {
            Owner = owner;
            Trait = trait;
        }
        #endregion
    }
}
