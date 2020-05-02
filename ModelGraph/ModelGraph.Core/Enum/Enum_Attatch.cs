
namespace ModelGraph.Core
{/*
 */
    public class Enum_Attach : EnumZ
    {
        internal override IdKey ViKey => IdKey.AttatchEnum;

        #region Constructor  ==================================================
        internal Enum_Attach(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            Add(new PairZ(this, IdKey.Attatch_Normal));
            Add(new PairZ(this, IdKey.Attatch_Radial));
            Add(new PairZ(this, IdKey.Attatch_RightAngle));
            Add(new PairZ(this, IdKey.Attatch_SkewedAngle));
        }
        #endregion
    }
}
