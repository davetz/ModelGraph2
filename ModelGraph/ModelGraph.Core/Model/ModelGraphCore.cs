using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{
    public static class ModelGraphCore
    {
        public static IRootModel CreateRootModel()
        {
            return new RootTreeModel();
        }
    }
}
