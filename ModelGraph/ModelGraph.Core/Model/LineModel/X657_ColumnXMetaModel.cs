
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class X657_ColumnXMetaModel : LineModel
    {
        internal X657_ColumnXMetaModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ColumnXMetaModel;
        public override bool CanExpandRight => true;

        public override (string kind, string name, int count) GetLineParms(Chef chef)
        {
            var (kind, name) = Item.GetKindNameId(chef);
            return (kind, name, 0);
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
