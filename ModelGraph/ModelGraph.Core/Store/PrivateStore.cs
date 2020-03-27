
namespace ModelGraph.Core
{
    public class PrivateStore<T> : StoreOf<T> where T : Item
    {
        public PrivateStore(Chef owner, Trait trait, int capacity = 0) : base(owner,trait, capacity) { }
    }
}
