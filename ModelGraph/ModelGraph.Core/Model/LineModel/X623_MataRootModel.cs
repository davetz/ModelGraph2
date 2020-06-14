using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class X623_MetaRootModel : LineModel
    {
        internal X623_MetaRootModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.MetadataRoot_Model;
        public override bool CanExpandLeft => true;

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;
            var root = DataRoot;

            new X623_ViewXListMetaModel(this, root.Get<ViewXRoot>());
            new X642_EnumXListMetaModel(this, root.Get<EnumXRoot>());
            new X643_TableXListMetaModel(this, root.Get<TableXRoot>());
            new X644_GraphXListMetaModel(this, root.Get<GraphXRoot>());
            new X740_InternalStoreListModel(this, Item);// TODO: FIX THIS***************

            IsExpandedLeft = true;
            return true;
        }
    }
}
