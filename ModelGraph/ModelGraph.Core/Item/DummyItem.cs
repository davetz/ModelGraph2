
namespace ModelGraph.Core
{
    public class DummyItem : Item
    {
        internal override bool IsReference => true;
        internal DummyItem(Chef owner)
        {
            Owner = owner;
            Trait = Trait.Dummy;
        }
    }
}
