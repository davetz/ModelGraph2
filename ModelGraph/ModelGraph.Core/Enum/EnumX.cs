
namespace ModelGraph.Core
{
    public class EnumX : StoreOf<PairX>
    {
        internal string Name;
        internal string Summary;
        internal string Description;

        #region Constructors  =================================================
        internal EnumX(EnumXStore owner, bool autoExpandRight = false)
        {
            Owner = owner;
            IdKey = IdKey.EnumX;

            if (autoExpandRight) AutoExpandRight = true;
            owner.Add(this);
        }
        #endregion
    }
}
