
namespace ModelGraph.Core
{
    public class EnumX : StoreOf<PairX>
    {
        internal string Name;
        internal string Summary;
        internal string Description;

        #region Constructors  =================================================
        internal EnumX(EnumX owner, bool autoExpandRight = false)
        {
            Owner = owner;
            Trait = Trait.EnumX;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        #endregion
    }
}
