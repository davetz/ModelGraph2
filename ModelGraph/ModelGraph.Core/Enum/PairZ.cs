

namespace ModelGraph.Core
{/*
 */
    public class PairZ : Item
    {
        internal PairZ(EnumZ owner, IdKey idKe)
        {
            Owner = owner;
            IdKey = idKe;

            owner.Add(this);
        }

        internal int EnumKey => (int)(IdKey & IdKey.EnumMask);
    }
}
