using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class MetaDataRootModel_623 : LineModel
    {
        internal MetaDataRootModel_623(RootModel_612 owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.MetaDataRootModel_623;
        public override bool CanExpandLeft => true;

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;
            var root = DataRoot;

            new ViewListModel_631(this, root.Get<ViewXRoot>());
            new EnumListModel_624(this, root.Get<EnumXRoot>());
            new TableListModel_643(this, root.Get<TableXRoot>());
            new GraphListModel_644(this, root.Get<GraphXRoot>());
            new InternalRootModel_7F0(this, Item);// TODO: FIX THIS***************

            IsExpandedLeft = true;
            return true;
        }
    }
}
