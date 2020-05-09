
namespace ModelGraph.Core
{
    public class X654_TableXMetaModel : LineModel
    {
        internal X654_TableXMetaModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.TableXMetaModel;
        public override bool CanExpandLeft => true;

        public override (string kind, string name, int count) GetLineParms(Chef chef)
        {
            var (kind, name) = Item.GetKindNameId(chef);
            return (kind, name, 0);
        }

        internal override bool ToggleLeft()
        {
            var chef = DataChef;

            if (IsExpandedLeft)
            {
                IsExpandedLeft = false;

                DiscardChildren();
            }
            else
            {
                IsExpandedLeft = true;

                new X661_ColumnXListMetaModel(this, Item);
            }

            return true;
        }


        //

    }
}
