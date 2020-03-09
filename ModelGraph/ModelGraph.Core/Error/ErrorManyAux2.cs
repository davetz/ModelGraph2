﻿using System.Collections.Generic;

namespace ModelGraph.Core
{
    internal class ErrorManyAux2 : ErrorMany
    {
        internal Item Aux1;
        internal Item Aux2;

        #region Constructor  ==================================================
        internal ErrorManyAux2(StoreOfOld<Error> owner, Item item, Item aux1, Item aux2, Trait trait, string text = null) : base(owner, item, trait, text)
        {
            Aux1 = aux1;
            Aux2 = aux2;
        }
        #endregion
    }
}
