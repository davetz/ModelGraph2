﻿
namespace ModelGraph.Core
{
    public class Relation_Store_ColumnX : RelationOf<Store,ColumnX>
    {
        internal override IdKey ViKey => IdKey.Store_ColumnX;

        internal Relation_Store_ColumnX(StoreOf_Relation owner)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = true;
            Initialize(25, 25);

            owner.Add(this);
        }
    }
}
