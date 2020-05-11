
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class X643_TableXListMetaModel : LineModel
    {
        internal X643_TableXListMetaModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.TableXListMetaModel;
        public override bool CanExpandLeft => true;

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;

            IsExpandedLeft = true;

            var st = Item as StoreOf_TableX;
            foreach (var tx in st.Items)
            {
                new X654_TableXMetaModel(this, tx);
            }

            return true;
        }

        internal override bool Validate(Dictionary<Item, LineModel> prev)
        {
            if (!IsExpanded) return false;
            if (ChildDelta == Item.ChildDelta) return false;
            ChildDelta = Item.ChildDelta;

            prev.Clear();
            foreach (var child in Items)
            {
                prev[child.Item] = child;
            }
            CovertClear();

            var st = Item as StoreOf_TableX;
            foreach (var tx in st.Items)
            {
                if (prev.TryGetValue(tx, out LineModel m))
                {
                    CovertAdd(m);
                    prev.Remove(m);
                }
                else
                    new X655_GraphXMetaModel(this, tx);
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
