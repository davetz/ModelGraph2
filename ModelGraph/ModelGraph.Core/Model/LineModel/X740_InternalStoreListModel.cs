
namespace ModelGraph.Core
{
    public class X740_InternalStoreListModel : LineModel
    {
        internal X740_InternalStoreListModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.InternalStoreListModel;

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
