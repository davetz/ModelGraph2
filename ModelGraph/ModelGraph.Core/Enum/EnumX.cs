using System;

namespace ModelGraph.Core
{/*
 */
    public class EnumX : StoreOfOld<PairX>
    {
        internal string Name;
        internal string Summary;
        internal string Description;

        #region Constructors  =================================================
        internal EnumX(StoreOfOld<EnumX> owner)
        {
            Owner = owner;
            Trait = Trait.EnumX;
            Guid = Guid.NewGuid();
            AutoExpandRight = true;

            owner.Add(this);
        }
        internal EnumX(StoreOfOld<EnumX> owner, Guid guid)
        {
            Owner = owner;
            Trait = Trait.EnumX;
            Guid = guid;

            owner.Add(this);
        }
        #endregion
    }
}
