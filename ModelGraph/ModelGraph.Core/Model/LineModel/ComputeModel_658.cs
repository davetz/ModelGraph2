
namespace ModelGraph.Core
{
    public class ComputeModel_658 : LineModel
    {//============================================== In the MetaDataRoot hierarchy  ==============
        internal ComputeModel_658(ComputeListModel_666 owner, ComputeX item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ComputeModel_658;
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
