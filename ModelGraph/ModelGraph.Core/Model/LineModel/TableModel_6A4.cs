
namespace ModelGraph.Core
{
    public class TableModel_6A4 : LineModel
    {//==================================== In the ModelingRoot hierarchy  ==============
        internal TableModel_6A4(TableListModel_647 owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.TableModel_6A4;
        public override bool CanExpandLeft => TotalCount > 0;
        public override int TotalCount => ItemStore.Count;

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;

            //new X661_ColumnXListMetaModel(this, Item);

            IsExpandedLeft = true;
            return true;
        }
    }
}
