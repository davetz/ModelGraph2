using System;

namespace ModelGraph.Core
{/*
 */
    public class TableX : StoreOf<RowX>
    {
        internal string Name;
        internal string Summary;
        internal string Description;

        #region Constructors  =================================================
        internal TableX(StoreOf<TableX> owner)
        {
            Owner = owner;
            Trait = Trait.TableX;
            AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion
    }
}
