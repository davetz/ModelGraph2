
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class ColumnListModel_661 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ColumnListModel_661(TableModel_654 owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ColumnListModel_661;
        public override bool CanExpandLeft => TotalCount > 0;


        internal override string GetFilterSortId(Root root) => GetSingleNameId(root);
        public override int TotalCount => DataRoot.Get<Relation_Store_ColumnX>().ChildCount(Item);

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;
            {
                IsExpandedLeft = true;

                if (DataRoot.Get<Relation_Store_ColumnX>().TryGetChildren(Item, out IList<ColumnX> cxList))
                {
                    foreach (var cx in cxList)
                    {
                        new ColumnModel_657(this, cx);
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
            var anyChange = false;
            if (IsExpanded)
            {
                if (ChildDelta != Item.ChildDelta)
                {
                    ChildDelta = Item.ChildDelta;

                    if (!DataRoot.Get<Relation_Store_ColumnX>().TryGetChildren(Item, out IList<ColumnX> cxList))
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
                            new ColumnModel_657(this, cx);
                            anyChange = true;
                        }
                    }

                    if (prev.Count > 0)
                    {
                        anyChange = true;
                        foreach (var model in prev.Values) { model.Discard(); }
                    }
                }

                foreach (var child in Items)
                {
                    anyChange |= child.Validate(treeRoot, prev);
                }
            }
            return anyChange;
        }
    }
}
