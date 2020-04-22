using System;

namespace ModelGraph.Core
{
    public class ViewX : Item
    {
        internal string Name;
        internal string Summary;
        internal string Description;

        #region Constructor  ======================================================
        internal ViewX(ViewXDomain owner, bool autoExpandRight = false)
        {
            Owner = owner;
            IdKey = IdKey.ViewX;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        #endregion
    }
}
