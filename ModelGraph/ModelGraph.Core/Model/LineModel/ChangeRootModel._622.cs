
namespace ModelGraph.Core
{
    public class ChangeRootModel_622 : LineModel
    {
        internal ChangeRootModel_622(RootModel_612 owner, ChangeRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ChangeRootModel_622;

        public override bool CanExpandLeft => (Item as ChangeRoot).Count > 0;

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;

            IsExpandedLeft = true;

            var csStore = Item as ChangeRoot;
            foreach (var cs in csStore.Items)
            {
                new ChangeSetModel_628(this, cs);
            }

            return true;
        }


    }
}
