
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class TableModel_654 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal TableModel_654(TableListModel_643 owner, TableX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.TableModel_654;

        public override bool CanExpandLeft => true;
        public override bool CanExpandRight => true;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);
        public override string GetSummaryId(Root root) => Item.GetSummaryId(root);
        internal override string GetFilterSortId(Root root) => Item.GetSingleNameId(root);

        public override void GetMenuCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new RemoveCommand(this, () => root.Get<ChangeRoot>().RemoveItem(Item)));
        }

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;

            new ColumnListModel_661(this, Item);
            new ComputeListModel_666(this, Item);
            new ChildRelationListModel_662(this, Item);
            new ParentRelatationListModel_663(this, Item);

            IsExpandedLeft = true;
            return true;
        }

        internal override bool ExpandRight()
        {
            if (IsExpandedRight) return false;
            var root = DataRoot;

            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Summary>());
            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Name>());

            IsExpandedRight = true;
            return true;
        }
    }
}
