
namespace ModelGraph.Core
{
    public class X654_TableXMetaModel : LineModel
    {
        internal X654_TableXMetaModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.TableXMetaModel;
        public override bool CanExpandLeft => true;
        public override bool CanExpandRight => true;

        public override (string kind, string name, int count) GetLineParms(Chef chef)
        {
            var (kind, name) = Item.GetKindNameId(chef);
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
            var chef = DataChef;

            new X617_TextPropertyModel(this, Item, chef.Get<Property_Item_Summary>());
            new X617_TextPropertyModel(this, Item, chef.Get<Property_Item_Name>());

            IsExpandedRight = true;
            return true;
        }
    }
}
