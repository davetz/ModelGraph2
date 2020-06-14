
using System.Collections.Generic;
using System.Linq;

namespace ModelGraph.Core
{
    public class X657_ColumnXMetaModel : LineModel
    {
        internal X657_ColumnXMetaModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ColumnX_MetaModel;
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

            new X618_CheckPropertyModel(this, Item, root.Get<Property_ColumnX_IsChoice>());
            new X619_ComboPropertyModel(this, Item, root.Get<Property_ColumnX_ValueType>());
            new X617_TextPropertyModel(this, Item, root.Get<Property_Item_Summary>());
            new X617_TextPropertyModel(this, Item, root.Get<Property_Item_Name>());

            IsExpandedRight = true;
            return true;
        }
    }
}
