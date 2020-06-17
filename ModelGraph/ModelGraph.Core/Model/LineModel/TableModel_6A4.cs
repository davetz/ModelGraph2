
namespace ModelGraph.Core
{
    public class TableModel_6A4 : LineModel
    {//==================================== In the ModelingRoot hierarchy  ==============
        internal TableModel_6A4(TableListModel_647 owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.TableModel_6A4;
        public override bool CanExpandLeft => ItemStore.Count > 0;

        public override (string kind, string name, int count) GetLineParms(Root root)
        {
            var(kind, name) = Item.GetKindNameId(root);
            return (kind, name, ItemStore.Count);
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
