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
        internal override State State { get; set; }

        #region Constructors  =================================================
        internal ColumnX(StoreOf<ColumnX> owner, bool autoExpand = false)
        {
            Owner = owner;
            if (autoExpand) AutoExpandRight = true;

            Value = Value.Create(ValType.String);

            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.ColumnX;
        public override string GetSingleNameId(Root root) => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetParentNameId(Root root) => root.Get<Relation_Store_ColumnX>().TryGetParent(this, out Store p) ? p.GetSingleNameId(root) : GetKindId(root);
        public override string GetSummaryId(Root root) => Summary;
        public override string GetDescriptionId(Root root) => Description;
        #endregion

    }
}
