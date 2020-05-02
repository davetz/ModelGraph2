﻿
namespace ModelGraph.Core
{
    public class RelationX_RowX_RowX : RelationX<RowX,RowX>
    {
        internal override IdKey ViKey => IdKey.RowX_RowX;

        internal RelationX_RowX_RowX(StoreOf_RelationX owner, bool autoExpandRight = false)
        {
            Owner = owner;
            Pairing = Pairing.OneToMany;
            IsRequired = false;
            Initialize(25, 25);

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
    }
}