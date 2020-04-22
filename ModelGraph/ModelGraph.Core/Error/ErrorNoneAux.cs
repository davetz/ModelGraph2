using System.Collections.Generic;

namespace ModelGraph.Core
{
    internal class ErrorNoneAux : ErrorNone
    {
        internal Item Aux;

        #region Constructor  ==================================================
        internal ErrorNoneAux(StoreOf<Error> owner, Item item, Item aux, IdKey trait) : base(owner, item, trait)
        {
            Aux = aux;
        }
        #endregion
    }
}
