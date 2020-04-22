using System;

namespace ModelGraph.Core
{/*
 */
    public class EnumZ : StoreOf<PairZ>
    {
        #region Constructor  ==================================================
        internal EnumZ(StoreOf<EnumZ> owner, IdKey idKe)
        {
            Owner = owner;
            IdKey = idKe;
        }
        #endregion
    }
}
