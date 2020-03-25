
namespace ModelGraph.Core
{
    public class Dummy : Item
    {
        internal Dummy(Chef owner)
        {
            Owner = owner;
            Trait = Trait.Dummy;
            owner.DummyStore.Add(this);
        }
    }
}
