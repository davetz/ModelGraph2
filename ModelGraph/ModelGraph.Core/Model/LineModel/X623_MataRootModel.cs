using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class X623_MetaRootModel : LineModel
    {
        internal X623_MetaRootModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.MetadataRootModel;
        public override bool CanExpandLeft => true;

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;
            var chef = DataChef;

            new X623_ViewXListMetaModel(this, chef.Get<StoreOf_ViewX>());
            new X642_EnumXListMetaModel(this, chef.Get<StoreOf_EnumX>());
            new X643_TableXListMetaModel(this, chef.Get<StoreOf_TableX>());
            new X644_GraphXListMetaModel(this, chef.Get<StoreOf_GraphX>());
            new X740_InternalStoreListModel(this, Item);// TODO: FIX THIS***************

            IsExpandedLeft = true;
            return true;
        }
    }
}
