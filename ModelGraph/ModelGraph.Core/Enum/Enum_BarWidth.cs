
namespace ModelGraph.Core
{/*
 */
    public class Enum_BarWidth : EnumZ
    {
        internal override IdKey ViKey => IdKey.BarWidthEnum;

        #region Constructor  ==================================================
        internal Enum_BarWidth(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            Add(new PairZ(this, IdKey.BarWidth_Thin));
            Add(new PairZ(this, IdKey.BarWidth_Wide));
            Add(new PairZ(this, IdKey.BarWidth_ExtraWide));
        }
        #endregion
    }
}
