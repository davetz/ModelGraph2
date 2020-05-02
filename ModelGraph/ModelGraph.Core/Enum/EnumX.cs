
namespace ModelGraph.Core
{
    public class EnumX : StoreOf<PairX>
    {
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        #region Constructors  =================================================
        internal EnumX(StoreOf_EnumX owner, bool autoExpandRight = false)
        {
            Owner = owner;
            OldIdKey = IdKey.EnumX;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey ViKey => IdKey.EnumX;
        public override string GetSingleNameId(Chef chef) => string.IsNullOrWhiteSpace(Name) ? BlankName : Name;
        public override string GetSummaryId(Chef chef) => Summary;
        public override string GetDescriptionId(Chef chef) => Description;
        #endregion

    }
}
