using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class X655_GraphXMetaModel : LineModel
    {
        internal X655_GraphXMetaModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.GraphXMetaModel;
    }
}
