
namespace ModelGraph.Core
{
    public class Relation_Store_ParentRelation : RelationOf<Store,Relation>
    {
        internal override IdKey IdKey => IdKey.Store_ParentRelation;

        internal Relation_Store_ParentRelation(Root owner)
        {
            Owner = owner;
            Pairing = Pairing.ManyToMany;
            IsRequired = true;
            Initialize(25, 25);
        }
    }
}
