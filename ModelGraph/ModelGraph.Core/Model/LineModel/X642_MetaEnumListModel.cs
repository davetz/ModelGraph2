
namespace ModelGraph.Core
{
    public class X642_MetaEnumListModel : LineModel
    {
        internal X642_MetaEnumListModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.MetaEnumListModel;

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
