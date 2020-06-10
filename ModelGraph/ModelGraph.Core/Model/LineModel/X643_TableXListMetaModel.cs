
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class X643_TableXListMetaModel : LineModel
    {
        internal X643_TableXListMetaModel(LineModel owner, TableXRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.TableXListMetaModel;
        public override bool CanExpandLeft => true;


        public override (string kind, string name, int count) GetLineParms(Root root)
        {
            var (kind, name) = GetKindNameId(root);
            var st = Item as TableXRoot;

            return (kind, name, st.Count);
        }

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;

            IsExpandedLeft = true;

            var st = Item as TableXRoot;
            foreach (var tx in st.Items)
            {
                new X654_TableXMetaModel(this, tx);
            }

            return true;
        }

        public override void GetButtonCommands(Root root, List<LineCommand> list)
        {
            list.Clear();
            list.Add(new InsertCommand(this, () => ItemCreated.Record(root, new TableX(Item as TableXRoot, true))));
        }

        internal override bool Validate(Dictionary<Item, LineModel> prev)
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
                            new X655_GraphXMetaModel(this, tx);
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
