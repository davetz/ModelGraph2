
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class X661_ColumnXListMetaModel : LineModel
    {
        internal X661_ColumnXListMetaModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ColumnXListMetaModel;
        public override bool CanExpandLeft => DataChef.Get<Relation_Store_ColumnX>().ChildCount(Item) > 0;

        public override (string kind, string name, int count) GetLineParms(Chef chef)
        {
            var count = DataChef.Get<Relation_Store_ColumnX>().ChildCount(Item);
            var (kind, name) = GetKindNameId(chef);
            return (kind, name, count);
        }

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;
            {
                IsExpandedLeft = true;

                if (DataChef.Get<Relation_Store_ColumnX>().TryGetChildren(Item, out IList<ColumnX> cxList))
                {
                    foreach (var cx in cxList)
                    {
                        new X657_ColumnXMetaModel(this, cx);
                    }
                }
            }

            return true;
        }
        public override void GetButtonCommands(Chef chef, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new X035_InsertCommand(this, AddNewColumnX));
        }
        private void AddNewColumnX()
        {
            var chef = DataChef;
            var sto = Item as Store;
            var cx = new ColumnX(chef.Get<StoreOf_ColumnX>(), true);
            chef.ItemCreated(cx);
            chef.Get<Relation_Store_ColumnX>().AppendLink(sto, cx);
        }

        internal override bool Validate(Dictionary<Item, LineModel> prev)
        {
            var anyChange = false;
            if (IsExpanded)
            {
                if (ChildDelta != Item.ChildDelta)
                {
                    ChildDelta = Item.ChildDelta;

                    if (!DataChef.Get<Relation_Store_ColumnX>().TryGetChildren(Item, out IList<ColumnX> cxList))
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
