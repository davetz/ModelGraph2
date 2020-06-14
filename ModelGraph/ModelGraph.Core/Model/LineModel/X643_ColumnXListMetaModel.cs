﻿
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class X661_ColumnXListMetaModel : LineModel
    {
        internal X661_ColumnXListMetaModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ColumnXList_MetaModel;
        public override bool CanExpandLeft => DataRoot.Get<Relation_Store_ColumnX>().ChildCount(Item) > 0;

        public override (string kind, string name, int count) GetLineParms(Root root)
        {
            var count = DataRoot.Get<Relation_Store_ColumnX>().ChildCount(Item);
            var (kind, name) = GetKindNameId(root);
            return (kind, name, count);
        }

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;
            {
                IsExpandedLeft = true;

                if (DataRoot.Get<Relation_Store_ColumnX>().TryGetChildren(Item, out IList<ColumnX> cxList))
                {
                    foreach (var cx in cxList)
                    {
                        new X657_ColumnXMetaModel(this, cx);
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

        internal override bool Validate(Dictionary<Item, LineModel> prev)
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
                            new X657_ColumnXMetaModel(this, cx);
                            anyChange = true;
                        }
                    }

                    if (prev.Count > 0)
                    {
                        anyChange = true;
                        var models = prev.Values.ToArray();
                        foreach (var m in models) m.Discard();
                    }
                }

                foreach (var child in Items)
                {
                    anyChange |= child.Validate(prev);
                }
            }
            return anyChange;
        }
    }
}
