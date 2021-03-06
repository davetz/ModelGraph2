﻿
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class DiagStoreModel_7F3 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal DiagStoreModel_7F3(DiagPrimeStoreModel_7F1 owner, Item item) : base(owner, item) { }
        private Store ST => Item as Store;
        internal override IdKey IdKey => IdKey.DiagStoreModel_7F3;

        public override bool CanExpandLeft => TotalCount > 0;
        public override bool CanFilter => TotalCount > 1;
        public override bool CanSort => TotalCount > 1;
        public override int TotalCount => ItemStore.Count;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

        internal override bool ExpandLeft(Root root)
        {
            if (IsExpandedLeft) return false;

            IsExpandedLeft = true;

            foreach (var itm in ST.GetItems())
            {
                new DiagItemModel_7F2(this, itm);
            }

            return true;
        }

        internal override bool Validate(Root root, Dictionary<Item, LineModel> prev)
        {
            var viewListChanged = false;
            if (IsExpanded || AutoExpandLeft)
            {
                AutoExpandLeft = false;
                IsExpandedLeft = true;

                if (ChildDelta != Item.ChildDelta)
                {
                    ChildDelta = Item.ChildDelta;

                    prev.Clear();
                    foreach (var child in Items)
                    {
                        prev[child.Item] = child;
                    }
                    CovertClear();

                    foreach (var itm in ST.GetItems())
                    {
                        if (prev.TryGetValue(itm, out LineModel m))
                        {
                            CovertAdd(m);
                            prev.Remove(m.Item);
                        }
                        else
                        {
                            new DiagItemModel_7F2(this, itm);
                            viewListChanged = true;
                        }
                    }

                    if (prev.Count > 0)
                    {
                        viewListChanged = true;
                        foreach (var model in prev.Values) { model.Discard(); }
                    }
                }
            }
            return viewListChanged || base.Validate(root, prev);
        }
    }
}
