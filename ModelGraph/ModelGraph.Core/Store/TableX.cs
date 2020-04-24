using System;

namespace ModelGraph.Core
{
    public class TableX : StoreOf<RowX>
    {
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        #region Constructors  =================================================
        internal TableX(StoreOf<TableX> owner, bool autoExpand = false)
        {
            Owner = owner;
            OldIdKey = IdKey.TableX;
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey VKey => IdKey.TableX;
        internal override string SingleNameId => Name;
        internal override string SummaryId => Summary;
        internal override string DescriptionId => Description;
        #endregion
    }
}
