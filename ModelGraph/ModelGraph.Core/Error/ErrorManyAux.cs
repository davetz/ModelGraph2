using System.Collections.Generic;

namespace ModelGraph.Core
{
    internal class ErrorManyAux : ErrorMany
    {
        internal Item Aux;

        #region Constructor  ==================================================
        internal ErrorManyAux(StoreOfOld<Error> owner, Item item, Item aux1, Trait trait, string text = null) : base(owner, item, trait, text)
        {
            Aux = aux1;
        }
        #endregion
    }
}
