﻿
namespace ModelGraph.Core
{
    public class RelationX : RelationOf<RowX, RowX>
    {
        internal override bool IsExternal => true;
        internal string Name;
        internal string Summary;
        internal string Description;

        #region Constructors  =================================================
        internal RelationX(RelationXStore owner, bool autoExpandRight = false)
        {
            Owner = owner;
            Trait = Trait.RelationX;
            Pairing = Pairing.OneToMany;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        #endregion
    }
}
