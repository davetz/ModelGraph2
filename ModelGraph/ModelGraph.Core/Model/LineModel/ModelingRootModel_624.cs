
namespace ModelGraph.Core
{
    public class ModelingRootModel_624 : LineModel
    {
        internal ModelingRootModel_624(RootModel_612 owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ModelingRootModel_624;
        public override bool CanExpandLeft => true;

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;
            var root = DataRoot;

            new ViewListModel_63A(this, root.Get<ViewXRoot>());
            new TableListModel_647(this, root.Get<TableXRoot>());
            new GraphListModel_648(this, root.Get<GraphXRoot>());

            IsExpandedLeft = true;
            return true;
        }

    }
}
