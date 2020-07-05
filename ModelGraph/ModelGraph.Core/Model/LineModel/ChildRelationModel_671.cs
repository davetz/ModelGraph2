
namespace ModelGraph.Core
{
    public class ChildRelationModel_671 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ChildRelationModel_671(ChildRelationListModel_662 owner, Relation item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ChildRelationModel_671;
        private RelationX_RowX_RowX RX => Item as RelationX_RowX_RowX;


        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);


        public override bool CanExpandRight => true;
        internal override bool ExpandRight(Root root)
        {
            if (IsExpandedRight) return false;
            IsExpandedRight = true;

            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Summary>());
            new PropertyTextModel_617(this, Item, root.Get<Property_Item_Name>());

            return true;
        }
    }
}
