using System;

namespace ModelGraph.Core
{/*
 */
    public class EnumZ : StoreOf<PairZ>
    {
        #region Constructor  ==================================================
        internal EnumZ(StoreOf<EnumZ> owner, IdKey trait)
        {
            Owner = owner;
            Trait = trait;
        }
        #endregion
    }
}
