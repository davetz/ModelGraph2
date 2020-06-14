
namespace ModelGraph.Core
{
    public class X629_ItemChangedModel : LineModel
    {
        internal X629_ItemChangedModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.ItemChange_Model;

    }
}
