using System;

namespace ModelGraph.Core
{
    public class ComputeX : Property
    {
        internal const string DefaultSeparator = " : ";

        override internal string Name { get; set; }
        override internal string Summary { get; set; }
        override internal string Description { get; set; }

        internal string Separator = DefaultSeparator;

        internal CompuType CompuType; // type of computation

        #region Constructors  =================================================
        internal ComputeX(StoreOf<ComputeX> owner, bool autoExpand = false)
        {
            Owner = owner;
            Trait = Trait.ComputeX;

            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion

        #region Property/Methods  =============================================
        internal override bool HasItemName => false;
        internal override string GetItemName(Item itm) { return null; }
        #endregion
    }
}