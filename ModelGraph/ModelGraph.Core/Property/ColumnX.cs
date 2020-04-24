using System;

namespace ModelGraph.Core
{
    public class ColumnX : Property
    {
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        #region Constructors  =================================================
        internal ColumnX(StoreOf<ColumnX> owner, bool autoExpand = false)
        {
            Owner = owner;
            OldIdKey = IdKey.ColumnX;
            if (autoExpand) AutoExpandRight = true;

            Value = Value.Create(ValType.String);

            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey VKey => IdKey.ColumnX;
        internal override string GetSingleNameId(Chef chef) => Name;
        internal override string GetParentNameId(Chef chef) => chef.Store_ColumnX.TryGetParent(this, out Store p) ? p.GetSingleNameId(chef) : GetKindId(chef);
        internal override string GetSummaryId(Chef chef) => Summary;
        internal override string GetDescriptionId(Chef chef) => Description;
        #endregion

        #region Property  =====================================================
        internal override bool HasItemName => false;
        internal override string GetItemName(Item key) => null;
        #endregion
    }
}
