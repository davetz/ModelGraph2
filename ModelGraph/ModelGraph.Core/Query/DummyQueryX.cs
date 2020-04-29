using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{
    class DummyQueryX : QueryX
    {
        internal override IdKey ViKey => IdKey.DummyQueryX;
        internal DummyQueryX(Chef owner) //QueryXNode, referenced in GraphParms
        {
            Owner = owner;
            OldIdKey = IdKey.DummyQueryX;
        }
    }
}
