﻿
namespace ModelGraph.Core
{
    public class Relation_Store_ComputeX : RelationOf<Store,ComputeX>
    {
        internal override IdKey ViKey => IdKey.Store_ComputeX;

        internal Relation_Store_ComputeX(StoreOf_Relation owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}