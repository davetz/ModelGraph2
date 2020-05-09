using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class X644_GraphXListMetaModel : LineModel
    {
        internal X644_GraphXListMetaModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.GraphXListMetaModel;

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

            var st = Item as StoreOf_GraphX;
            foreach (var gx in st.Items)
            {
                if (prev.TryGetValue(gx, out LineModel m))
                {
                    CovertAdd(m);
                    prev.Remove(m);
                }
                else
                    new X655_GraphXMetaModel(this, gx);
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
