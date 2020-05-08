
namespace ModelGraph.Core
{
    public class X623_MetadataRootModel : LineModel
    {
        internal X623_MetadataRootModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.MetadataRootModel;

        public override (string kind, string name, int count) GetLineParms(Chef chef)
        {
            var (kind, name) = GetKindNameId(chef);
            return (kind, name, Count);
        }

        internal override (bool anyChange, int flatCount) Validate()
        {
            bool anyChange = Count != 5;
            if (Count == 5) return (true, 5);
            {
                new X623_MetaViewXViewListModel(this, Item);
                new X642_MetaEnumListModel(this, Item);
                new X643_MetaTableListModel(this, Item);
                new X644_MetaGraphListModel(this, Item);
                new X740_InternalStoreListModel(this, Item);
            }

            var flatCount = Count;
            foreach (var child in Items)
            {
                var (childChanged, childFlatCount) = child.Validate();
                anyChange |= childChanged;
                flatCount += childFlatCount;
            }
            return (anyChange, flatCount);
        }
    }
}
