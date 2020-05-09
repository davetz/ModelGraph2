using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class X612_DataChefModel : LineModel
    {
        public override bool CanExpandLeft => true;

        internal X612_DataChefModel(LineModel owner, Item item) : base(owner, item) { }

        internal override IdKey IdKey => IdKey.DataChefModel;
        
        public override (string kind, string name, int count) GetLineParms(Chef chef)
        {
            var (kind, name) = GetKindNameId(chef);
            return (kind, name, Count);
        }

        /// <summary>Recursive traversal of model hierachy</summary>
        internal override bool Validate(Dictionary<Item,LineModel> prev)
        {
            bool anyChange = Count != 5;
            if (Count != 5)
            {
                new X620_ParmRootModel(this, Item);
                new X621_ErrorRootModel(this, Item);
                new X622_ChangeRootModel(this, Item);
                new X623_MetaRootModel(this, Item);
                new X624_ModelRootModel(this, Item);
            }

            foreach (var child in Items)
            {
                anyChange |= child.Validate(prev);
            }
            return anyChange;
        }
    }
}
