
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class PrivateStore<T> : StoreOf<T> where T : Item
    {
        public PrivateStore(Chef owner, Trait trait, int capacity)
        {
            Owner = owner;
            Trait = trait;
            SetCapacity(capacity);

            owner.PrivateStores.Add(this);  // collect by stores type
        }
    }
}
