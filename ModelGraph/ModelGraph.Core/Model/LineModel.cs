using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{
    public class LineModel : Item
    {
        internal Item Item;             // target item
        internal LineAction Get;        // custom actions for this LineModel

        public string ViewFilter;       // UI imposed Kind/Name filter
        public byte Depth;              // depth of simulated tree hierarchy
    }
}
