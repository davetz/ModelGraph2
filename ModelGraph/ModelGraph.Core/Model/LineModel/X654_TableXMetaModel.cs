
namespace ModelGraph.Core
{
    public class X654_TableXMetaModel : LineModel
    {
        internal X654_TableXMetaModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.TableX_MetaModel;
        public override bool CanExpandLeft => true;
        public override bool CanExpandRight => true;

        public override (string kind, string name, int count) GetLineParms(Root root)
        {
            var (kind, name) = Item.GetKindNameId(root);
            return (kind, name, 0);
        }

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;

            new X661_ColumnXListMetaModel(this, Item);

            IsExpandedLeft = true;
            return true;
        }
        internal override bool ExpandRight()
        {
            if (IsExpandedRight) return false;
            var root = DataRoot;

            new X617_TextPropertyModel(this, Item, root.Get<Property_Item_Summary>());
            new X617_TextPropertyModel(this, Item, root.Get<Property_Item_Name>());

            IsExpandedRight = true;
            return true;
        }
    }
}
