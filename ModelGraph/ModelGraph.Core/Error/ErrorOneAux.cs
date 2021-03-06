﻿using System.Collections.Generic;

namespace ModelGraph.Core
{
    internal class ErrorOneAux : ErrorOne
    {
        internal Item Aux;

        #region Constructor  ==================================================
        internal ErrorOneAux(StoreOf<Error> owner, Item item, Item aux, IdKey idKe, string text = null) : base(owner,item,idKe, text)
        {
            Aux = aux;
        }
        #endregion
    }
}
