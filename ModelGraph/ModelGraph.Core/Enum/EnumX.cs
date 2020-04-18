
namespace ModelGraph.Core
{
    public class EnumX : StoreOf<PairX>
    {
        internal override bool IsExternal => true;
        internal string Name;
        internal string Summary;
        internal string Description;

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
