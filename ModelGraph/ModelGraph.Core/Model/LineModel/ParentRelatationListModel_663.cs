
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class ParentRelatationListModel_663 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ParentRelatationListModel_663(TableModel_654 owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ParentRelatationListModel_663;
        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => true;
        public override bool CanSort => true;

        internal override string GetFilterSortId(Root root) => GetSingleNameId(root);
        public override int TotalCount => DataRoot.Get<Relation_Store_ParentRelation>().ChildCount(Item);

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;
            {
                IsExpandedLeft = true;

                if (DataRoot.Get<Relation_Store_ParentRelation>().TryGetChildren(Item, out IList<Relation> cxList))
                {
                    foreach (var cx in cxList)
                    {
                        //new ColumnModel_657(this, cx);
                    }
                }
            }

            return true;
        }
        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, AddNewColumnX));
        }
        private void AddNewColumnX()
        {
            var root = DataRoot;
            var cx = new ColumnX(root.Get<ColumnXRoot>(), true);
            var sto = Item as Store;

            // the data root implements undo/redo functionality
            ItemCreated.Record(root, cx);
            ItemLinked.Record(root, root.Get<Relation_Store_ColumnX>(), sto, cx);
        }

        internal override bool Validate(TreeModel treeRoot, Dictionary<Item, LineModel> prev)
        {
            var viewListChange = false;
            if (IsExpanded || AutoExpandLeft)
            {
                AutoExpandLeft = false;
                IsExpandedLeft = true;

                if (ChildDelta != Item.ChildDelta)
                {
                    ChildDelta = Item.ChildDelta;

                    if (!DataRoot.Get<Relation_Store_ParentRelation>().TryGetChildren(Item, out IList<Relation> cxList))
                    {
                        IsExpandedLeft = false;
                        DiscardChildren();
                        CovertClear();
                        return true;
                    }

                    prev.Clear();
                    foreach (var child in Items)
                    {
                        prev[child.Item] = child;
                    }
                    CovertClear();

                    foreach (var cx in cxList)
                    {
                        if (prev.TryGetValue(cx, out LineModel m))
                        {
                            CovertAdd(m);
                            prev.Remove(m.Item);
                        }
                        else
                        {
                            //new ColumnModel_657(this, cx);
                            viewListChange = true;
                        }
                    }

                    if (prev.Count > 0)
                    {
                        viewListChange = true;
                        foreach (var model in prev.Values) { model.Discard(); }
                    }
                }
            }
            return viewListChange || base.Validate(treeRoot, prev);
        }
    }
}
