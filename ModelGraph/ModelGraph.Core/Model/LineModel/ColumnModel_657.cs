
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class ColumnModel_657 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ColumnModel_657(ColumnListModel_661 owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ColumnModel_657;
        public override bool CanExpandRight => true;

        public override (string kind, string name, int count) GetLineParms(Root root)
        {
            var (kind, name) = Item.GetKindNameId(root);
            return (kind, name, 0);
        }

        internal override bool ExpandRight()
        {
            if (IsExpandedRight) return false;
            var root = DataRoot;

            new PropertyCheckModel_618(this, Item, root.Get<Property_ColumnX_IsChoice>());
            new PropertyComboModel_619(this, Item, root.Get<Property_ColumnX_ValueType>());
            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Summary>());
            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Name>());

            IsExpandedRight = true;
            return true;
        }
    }
}
