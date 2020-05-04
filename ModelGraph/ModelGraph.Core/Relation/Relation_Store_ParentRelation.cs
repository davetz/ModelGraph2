﻿
namespace ModelGraph.Core
{
    public class Relation_Store_ParentRelation : RelationOf<Store,Relation>
    {
        internal override IdKey IdKey => IdKey.Store_ParentRelation;

        internal Relation_Store_ParentRelation(StoreOf_Relation owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
