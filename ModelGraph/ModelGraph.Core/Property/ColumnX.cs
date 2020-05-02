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
        internal override IdKey ViKey => IdKey.ColumnX;
        public override string GetSingleNameId(Chef chef) => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetParentNameId(Chef chef) => chef.Get<Relation_Store_ColumnX>().TryGetParent(this, out Store p) ? p.GetSingleNameId(chef) : GetKindId(chef);
        public override string GetSummaryId(Chef chef) => Summary;
        public override string GetDescriptionId(Chef chef) => Description;
        #endregion

    }
}
