
namespace ModelGraph.Core
{
    public class X740_InternalStoreListModel : LineModel
    {
        internal X740_InternalStoreListModel(LineModel owner, Item item) : base(owner, item) { }
        internal override IdKey IdKey => IdKey.InternalStoreList_Model;
    }
}
