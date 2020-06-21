using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class GraphListModel_644 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal GraphListModel_644(MetaDataRootModel_623 owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.GraphListModel_644;

        internal override bool Validate(TreeModel treeRoot, Dictionary<Item, LineModel> prev)
        {
            var anyChange = false;
            if (!IsExpanded) return false;
            if (ChildDelta == Item.ChildDelta) return false;
            ChildDelta = Item.ChildDelta;

            prev.Clear();
            foreach (var child in Items)
            {
                prev[child.Item] = child;
            }
            CovertClear();

            var st = Item as GraphXRoot;
            foreach (var gx in st.Items)
            {
                if (prev.TryGetValue(gx, out LineModel m))
                {
                    CovertAdd(m);
                    prev.Remove(m);
                }
                else
                    new GraphModel_655(this, gx);
            }

            if (prev.Count > 0)
            {
                anyChange = true;
                foreach (var model in prev.Values) { model.Discard(); }
            }

            return anyChange;
        }
    }
}
