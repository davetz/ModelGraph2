
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
        internal EnumX(EnumXDomain owner, bool autoExpandRight = false)
        {
            Owner = owner;
            OldIdKey = IdKey.EnumX;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey VKey => IdKey.EnumX;
        internal override string SingleNameId => Name;
        internal override string SummaryId => Summary;
        internal override string DescriptionId => Description;
        #endregion

    }
}
