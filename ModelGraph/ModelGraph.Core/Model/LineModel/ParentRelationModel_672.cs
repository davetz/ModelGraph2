
namespace ModelGraph.Core
{
    public class ParentRelationModel_672 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ParentRelationModel_672(ParentRelatationListModel_663 owner, Relation item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ParentRelationModel_672;
        public override bool CanExpandRight => true;

        public override (string, string) GetKindNameId(Root root) => Item.GetKindNameId(root);

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
