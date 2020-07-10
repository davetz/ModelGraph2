
namespace ModelGraph.Core
{
    public class Relation_StoreX_ParentRelation : RelationOf<Store,Relation>
    {
        internal override IdKey IdKey => IdKey.StoreX_ParentRelation;

        internal Relation_StoreX_ParentRelation(RelationRoot owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);
            owner.Add(this);
        }
    }
}
