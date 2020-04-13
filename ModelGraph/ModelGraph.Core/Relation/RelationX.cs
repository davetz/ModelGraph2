
namespace ModelGraph.Core
{
    public class RelationX : RelationOf<RowX, RowX>
    {
        override internal string Name { get; set; }
        override internal string Summary { get; set; }
        override internal string Description { get; set; }

        #region Constructors  =================================================
        internal RelationX(RelationXStore owner, bool autoExpandRight = false)
        {
            Owner = owner;
            Trait = Trait.RelationX;
            Pairing = Pairing.OneToMany;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        #endregion
    }
}
