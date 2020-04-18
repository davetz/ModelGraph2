using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{
    class DummyQueryX : QueryX
    {
        internal override bool IsExternal => false;
        internal override bool IsReference => true;
        internal DummyQueryX(Chef owner) //QueryXNode, referenced in GraphParms
        {
            Owner = owner;
            Trait = Trait.NodeParm;
        }
    }
}
