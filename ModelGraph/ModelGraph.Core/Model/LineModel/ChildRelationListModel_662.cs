using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class ChildRelationListModel_662 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ChildRelationListModel_662(TableModel_654 owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ChildRelationListModel_662;
        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => true;
        public override bool CanSort => true;

        internal override string GetFilterSortId(Root root) => GetSingleNameId(root);
        public override int TotalCount => DataRoot.Get<Relation_Store_ChildRelation>().ChildCount(Item);

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;
            {
                IsExpandedLeft = true;

                if (DataRoot.Get<Relation_Store_ColumnX>().TryGetChildren(Item, out IList<ColumnX> cxList))
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

                    if (!DataRoot.Get<Relation_Store_ChildRelation>().TryGetChildren(Item, out IList<Relation> cxList))
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
