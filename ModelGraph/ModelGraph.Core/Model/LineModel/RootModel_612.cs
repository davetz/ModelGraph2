using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class RootModel_612 : LineModel
    {
        internal override IdKey IdKey => IdKey.RootModel_612;
        public override bool CanExpandLeft => true;

        internal RootModel_612(LineModel owner, Root item) : base(owner, item) 
        {
            var root = Item as Root;
            new RootParamModel_620(this, Item);
            new ErrorRootModel_621(this, Item);
            new ChangeRootModel_622(this, root.Get<ChangeRoot>());
            new MetaDataRootModel_623(this, Item);
            new ModelingRootModel_624(this, Item);

            IsExpandedLeft = true;
        }
        
        public override (string kind, string name, int count) GetLineParms(Root root)
        {
            var (kind, name) = GetKindNameId(root);
            return (kind, name, Count);
        }
    }
}
