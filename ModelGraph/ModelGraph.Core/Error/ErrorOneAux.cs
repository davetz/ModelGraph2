using System.Collections.Generic;

namespace ModelGraph.Core
{
    internal class ErrorOneAux : ErrorOne
    {
        internal Item Aux;

        #region Constructor  ==================================================
        internal ErrorOneAux(StoreOfOld<Error> owner, Item item, Item aux, Trait trait, string text = null) : base(owner,item,trait, text)
        {
            Aux = aux;
        }
        #endregion
    }
}
