
namespace ModelGraph.Core
{
    public class X612_DataChefModel : LineModel
    {
        public override bool CanExpandLeft => true;

        internal X612_DataChefModel(LineModel owner, Item item) : base(owner, item) { Validate(); }

        internal override IdKey IdKey => IdKey.DataChefModel;
        public override (string kind, string name, int count) GetLineParms(Chef chef)
        {
            var (kind, name) = GetKindNameId(chef);
            return (kind, name, Count);
        }

        /// <summary>Recursive traversal of model hierachy</summary>
        internal override (bool anyChange, int flatCount) Validate()
        {
            bool anyChange = Count != 5 || !IsExpanded;
            if (Count != 5)
            {
                new X620_ParmRootModel(this, Item);
                new X621_ErrorRootModel(this, Item);
                new X622_ChangeRootModel(this, Item);
                new X623_MetadataRootModel(this, Item);
                new X624_ModelingRootModel(this, Item);
            }
            var flatCount = Count;
            foreach (var child in Items)
            {
                var (childChanged, childFlatCount) = child.Validate();
                anyChange |= childChanged;
                flatCount += childFlatCount;
            }
            IsExpandedLeft = true;
            return (anyChange, flatCount);
        }
    }
}
