using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class X612_DataRootModel : LineModel
    {
        internal override IdKey IdKey => IdKey.DataRoot_Model;
        public override bool CanExpandLeft => true;

        internal X612_DataRootModel(LineModel owner, Root item) : base(owner, item) 
        {
            var root = Item as Root;
            new X620_ParmRootModel(this, Item);
            new X621_ErrorRootModel(this, Item);
            new X622_ChangeRootModel(this, root.Get<ChangeRoot>());
            new X623_MetaRootModel(this, Item);
            new X624_ModelRootModel(this, Item);

            IsExpandedLeft = true;
        }
        
        public override (string kind, string name, int count) GetLineParms(Root root)
        {
            var (kind, name) = GetKindNameId(root);
            return (kind, name, Count);
        }
    }
}
