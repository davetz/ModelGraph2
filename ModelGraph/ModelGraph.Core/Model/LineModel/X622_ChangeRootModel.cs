
namespace ModelGraph.Core
{
    public class X622_ChangeRootModel : LineModel
    {
        internal X622_ChangeRootModel(LineModel owner, ChangeRoot item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ChangeRootModel;

        public override bool CanExpandLeft => (Item as ChangeRoot).Count > 0;

        internal override bool ExpandLeft()
        {
            if (IsExpandedLeft) return false;

            IsExpandedLeft = true;

            var csStore = Item as ChangeRoot;
            foreach (var cs in csStore.Items)
            {
                new X628_ChangeSetModel(this, cs);
            }

            return true;
        }


    }
}
