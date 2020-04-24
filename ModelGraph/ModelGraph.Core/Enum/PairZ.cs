

namespace ModelGraph.Core
{/*
 */
    public class PairZ : Item
    {
        internal PairZ(EnumZ owner, IdKey idKe)
        {
            Owner = owner;
            OldIdKey = idKe;

            owner.Add(this);
        }

        internal int EnumKey => (int)(OldIdKey & IdKey.EnumMask);
    }
}
