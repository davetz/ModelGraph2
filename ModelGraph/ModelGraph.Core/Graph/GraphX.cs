﻿using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class GraphX : StoreOf<Graph>
    {
        internal override bool IsExternal => true;
        internal Color Color = new Color();
        internal HashSet<Store> NodeOwners = new HashSet<Store>();
        internal Dictionary<Item, Dictionary<QueryX, List<NodeEdge>>> Root_QueryX_Parms = new Dictionary<Item, Dictionary<QueryX, List<NodeEdge>>>(10);

        internal string Name;
        internal string Summary;
        internal string Description;
        internal byte SymbolSize = 48;
        internal byte ThinBusSize = 6;
        internal byte WideBusSize = 10;
        internal byte ExtraBusSize = 20;
        internal byte MinNodeSize = 4;
        internal byte TerminalLength = 10;
        internal byte TerminalSpacing = 8;
        internal byte SurfaceSkew = 4;
        internal byte TerminalSkew = 2;

        public List<(byte A, byte R, byte G, byte B)> ARGBList => Color.ARGBList;

        #region Constructors  =================================================
        internal GraphX(Store owner, bool autoExpand = false)
        {
            Owner = owner;
            Trait = Trait.GraphX;
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion

        #region Properties/Methods  ===========================================
        public float SymbolScale => SymbolSize;
        internal bool TryGetGraph(Item root, out Graph graph)
        {
            if (Count > 0)
            {
                foreach (var item in Items)
                {
                    var g = item as Graph;
                    if (g.SeedItem != root) continue;
                    graph = g;
                    return true;
                }
            }
            graph = null;
            return false;
        } 
        #endregion
    }
}
