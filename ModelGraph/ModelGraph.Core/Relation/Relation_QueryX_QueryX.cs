﻿
namespace ModelGraph.Core
{
    public class Relation_QueryX_QueryX : RelationOf<QueryX,QueryX>
    {
        internal override IdKey ViKey => IdKey.QueryX_QueryX;

        internal Relation_QueryX_QueryX(StoreOf_Relation owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}