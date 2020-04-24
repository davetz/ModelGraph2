using System;

namespace ModelGraph.Core
{
    public class ComputeX : Property
    {
        internal const string DefaultSeparator = " : ";
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        internal string Separator = DefaultSeparator;

        internal CompuType CompuType; // type of computation

        #region Constructors  =================================================
        internal ComputeX(StoreOf<ComputeX> owner, bool autoExpand = false)
        {
            Owner = owner;
            OldIdKey = IdKey.ComputeX;

            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey VKey => IdKey.ComputeX;
        internal override string SingleNameId => Name;
        internal override string ParentNameId => GetChef().Store_ComputeX.TryGetParent(this, out Store p) ? p.SingleNameId : KindId;
        internal override string SummaryId => Summary;
        internal override string DescriptionId => Description;
        #endregion

        #region Property/Methods  =============================================
        internal override bool HasItemName => false;
        internal override string GetItemName(Item itm) { return null; }
        #endregion
    }
}