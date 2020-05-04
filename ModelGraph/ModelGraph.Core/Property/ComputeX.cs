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

            if (autoExpand) AutoExpandRight = true;

            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.ComputeX;
        public override string GetSingleNameId(Chef chef) => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetParentNameId(Chef chef) => chef.Get<Relation_Store_ComputeX>().TryGetParent(this, out Store p) ? p.GetSingleNameId(chef) : GetKindId(chef);
        public override string GetSummaryId(Chef chef) => Summary;
        public override string GetDescriptionId(Chef chef) => Description;
        #endregion
    }
}