
namespace ModelGraph.Core
{
    public class RelationX<T1,T2> : RelationOf<T1, T2> where T1 : Item where T2 : Item
    {
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        #region Constructors  =================================================
        internal RelationX(RelationXDomain owner, IdKey idKe, bool autoExpandRight = false)
        {
            Owner = owner;
            IdKey = idKe;
            Pairing = Pairing.OneToMany;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        #endregion
    }

    public class RelationXO : RelationOf<RowX, RowX>
    {
        internal override string Name { get => _name; set => _name = value; }
        private string _name;
        internal override string Summary { get => _summary; set => _summary = value; }
        private string _summary;
        internal override string Description { get => _description; set => _description = value; }
        private string _description;

        #region Constructors  =================================================
        internal RelationXO(RelationXDomain owner, bool autoExpandRight = false)
        {
            Owner = owner;
            IdKey = IdKey.RelationX;
            Pairing = Pairing.OneToMany;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        #endregion
    }
}
