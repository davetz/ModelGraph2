
namespace ModelGraph.Core
{
    public class Relation_Store_QueryX : RelationOf<Store,QueryX>
    {
        internal override IdKey ViKey => IdKey.Store_QueryX;

        internal Relation_Store_QueryX(RelationDomain owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
