
namespace ModelGraph.Core
{
    public class DummyItem : Item
    {
        internal override IdKey ViKey => IdKey.DummyItem;
        internal DummyItem(Chef owner)
        {
            Owner = owner;
        }
    }
}
