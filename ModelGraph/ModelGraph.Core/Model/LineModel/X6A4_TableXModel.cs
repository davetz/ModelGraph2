
namespace ModelGraph.Core
{
    public class X6A4_TableXModel : LineModel
    {
        internal X6A4_TableXModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.TableX_Model;
        public override bool CanExpandLeft => Count > 0;

        public override (string kind, string name, int count) GetLineParms(Root root)
        {
            var(kind, name) = Item.GetKindNameId(root);
            return (kind, name, 0);
        }

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;

            //new X661_ColumnXListMetaModel(this, Item);

            IsExpandedLeft = true;
            return true;
        }
    }
}
