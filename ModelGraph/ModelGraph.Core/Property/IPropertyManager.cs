using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{
    internal interface IPropertyManager
    {
        Property[] GetPropreties(ItemModel model = null);
    }
}
