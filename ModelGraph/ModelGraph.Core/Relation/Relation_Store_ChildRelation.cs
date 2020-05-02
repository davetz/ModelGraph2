
namespace ModelGraph.Core
{
    public class Relation_Store_ChildRelation : RelationOf<Store,Relation>
    {
        internal override IdKey ViKey => IdKey.Store_ChildRelation;

        internal Relation_Store_ChildRelation(StoreOf_Relation owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
