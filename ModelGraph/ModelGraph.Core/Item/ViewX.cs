using System;

namespace ModelGraph.Core
{
    public class ViewX : Item
    {
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        #region Constructor  ======================================================
        internal ViewX(ViewXDomain owner, bool autoExpandRight = false)
        {
            Owner = owner;
            OldIdKey = IdKey.ViewX;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey VKey => IdKey.ViewX;
        internal override string GetSingleNameId(Chef chef) => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        internal override string GetSummaryId(Chef chef) => Summary;
        internal override string GetDescriptionId(Chef chef) => Description;
        #endregion
    }
}
