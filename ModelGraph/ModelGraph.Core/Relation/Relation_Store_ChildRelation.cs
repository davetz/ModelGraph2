
namespace ModelGraph.Core
{
    public class Relation_Store_ChildRelation : RelationOf<Store,Relation>
    {
        internal override IdKey IdKey => IdKey.Store_ChildRelation;

        internal Relation_Store_ChildRelation(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.ManyToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
