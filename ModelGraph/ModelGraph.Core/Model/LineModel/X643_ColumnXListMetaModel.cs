
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

        internal override bool ToggleLeft()
        {
            if (IsExpandedLeft)
            {
                IsExpandedLeft = false;

                DiscardChildren();
            }
            else
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

        internal override bool Validate(Dictionary<Item, LineModel> prev)
        {
            if (!IsExpanded) return false;
            if (ChildDelta == Item.ChildDelta) return false;
            ChildDelta = Item.ChildDelta;

            if (!DataChef.Get<Relation_Store_ColumnX>().TryGetChildren(Item, out IList<ColumnX> cxList))
            {
                IsExpandedLeft = false;
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
                    prev.Remove(m);
                }
                else
                    new X657_ColumnXMetaModel(this, cx);
            }

            if (prev.Count > 0)
            {
                var models = prev.Values.ToArray();
                foreach (var m in models) m.Discard();
            }

            return true;
        }
    }
}
