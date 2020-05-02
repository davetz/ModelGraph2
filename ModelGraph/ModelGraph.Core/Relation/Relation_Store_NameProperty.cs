﻿
namespace ModelGraph.Core
{
    public class Relation_Store_NameProperty : RelationOf<Store,Property>
    {
        internal override IdKey ViKey => IdKey.Store_NameProperty;

        internal Relation_Store_NameProperty(StoreOf_Relation owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToOne;
            IsRequired = false;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}