using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class X612_DataChefModel : LineModel
    {
        internal override IdKey IdKey => IdKey.DataChefModel;
        public override bool CanExpandLeft => true;

        internal X612_DataChefModel(LineModel owner, Chef item) : base(owner, item) 
        {
            var chef = Item as Chef;
            new X620_ParmRootModel(this, Item);
            new X621_ErrorRootModel(this, Item);
            new X622_ChangeRootModel(this, chef.Get<ChangeRoot>());
            new X623_MetaRootModel(this, Item);
            new X624_ModelRootModel(this, Item);

            IsExpandedLeft = true;
        }
        
        public override (string kind, string name, int count) GetLineParms(Chef chef)
        {
            var (kind, name) = GetKindNameId(chef);
            return (kind, name, Count);
        }
    }
}
