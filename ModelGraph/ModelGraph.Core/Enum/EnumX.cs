
namespace ModelGraph.Core
{
    public class EnumX : StoreOf<PairX>
    {
        override internal string Name { get; set; }
        override internal string Summary { get; set; }
        override internal string Description { get; set; }

        #region Constructors  =================================================
        internal EnumX(EnumXStore owner, bool autoExpandRight = false)
        {
            Owner = owner;
            Trait = Trait.EnumX;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        #endregion
    }
}
