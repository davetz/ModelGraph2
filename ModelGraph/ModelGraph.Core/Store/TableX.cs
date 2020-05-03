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
            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey ViKey => IdKey.TableX;
        public override string GetSingleNameId(Chef chef) => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetSummaryId(Chef chef) => Summary;
        public override string GetDescriptionId(Chef chef) => Description;
        #endregion
    }
}
