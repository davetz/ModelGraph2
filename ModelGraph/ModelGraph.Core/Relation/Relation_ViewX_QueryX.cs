﻿
namespace ModelGraph.Core
{
    public class Relation_ViewX_QueryX : RelationOf<ViewX,QueryX>
    {
        internal override IdKey ViKey => IdKey.ViewX_QueryX;

        internal Relation_ViewX_QueryX(StoreOf_Relation owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToOne;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}