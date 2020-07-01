﻿
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class TableListModel_647 : LineModel
    {//============================================== In the ModelingRoot hierarchy  ==============
        internal TableListModel_647(ModelingRootModel_624 owner, TableXRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.TableListModel_647;
        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => TotalCount > 1;
        public override bool CanSort => TotalCount > 1;
        public override int TotalCount => ItemStore.Count;

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;

            IsExpandedLeft = true;

            var st = Item as TableXRoot;
            foreach (var tx in st.Items)
            {
                new TableModel_6A4(this, tx);
            }

            return true;
        }

        internal override bool Validate(TreeModel treeRoot, Dictionary<Item, LineModel> prev)
        {
            var anyChange = false;
            if (IsExpanded)
            {
                if (ChildDelta != Item.ChildDelta)
                {
                    ChildDelta = Item.ChildDelta;

                    prev.Clear();
                    foreach (var child in Items)
                    {
                        prev[child.Item] = child;
                    }
                    CovertClear();

                    var st = Item as TableXRoot;
                    foreach (var tx in st.Items)
                    {
                        if (prev.TryGetValue(tx, out LineModel m))
                        {
                            CovertAdd(m);
                            prev.Remove(m.Item);
                        }
                        else
                        {
                            new TableModel_6A4(this, tx);
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
