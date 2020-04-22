

namespace ModelGraph.Core
{/*
 */
    public class PairZ : Item
    {
        internal PairZ(EnumZ owner, IdKey trait)
        {
            Owner = owner;
            Trait = trait;

            owner.Add(this);
        }

        internal int EnumKey => (int)(Trait & IdKey.EnumMask);
    }
}
