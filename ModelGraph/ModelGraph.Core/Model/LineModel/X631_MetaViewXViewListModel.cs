﻿
namespace ModelGraph.Core
{
    public class X623_MetaViewXViewListModel : LineModel
    {
        internal X623_MetaViewXViewListModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.MetaViewViewListModel;

        public override (string kind, string name, int count) GetLineParms(Chef chef)
        {
            var (kind, name) = GetKindNameId(chef);
            return (kind, name, Count);
        }

        internal override (bool anyChange, int flatCount) Validate()
        {
            return (false, 0);
        }
    }
}
