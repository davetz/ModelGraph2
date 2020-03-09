using System.Collections.Generic;

namespace ModelGraph.Core
{
    internal class ErrorNone : Error
    {
        #region Constructor  ==================================================
        internal ErrorNone(StoreOfOld<Error> owner, Item item, Trait trait)
        {
            Owner = owner;
            Item = item;
            Trait = trait;
            owner.Add(this);
        }
        #endregion

        #region Overrides  ====================================================
        internal override void Add(string text) { }
        internal override void Clear() { }

        internal override int Count => 0;
        internal override string GetError(int index = 0) => string.Empty;
        #endregion
    }
}
