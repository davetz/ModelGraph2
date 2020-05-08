
namespace ModelGraph.Core
{
    public class X643_MetaTableListModel : LineModel
    {
        internal X643_MetaTableListModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.MetaTableListModel;

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
