
using System;

namespace ModelGraph.Core
{
    /// <summary>A graph element, either a node or an edge</summary>
    abstract public class NodeEdge : Item
    {
        internal override bool IsExternal => true;
    }
}
