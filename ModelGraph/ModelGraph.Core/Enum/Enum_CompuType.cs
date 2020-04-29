
namespace ModelGraph.Core
{/*
 */
    public class Enum_CompuType : EnumZ
    {
        internal override IdKey ViKey => IdKey.CompuTypeEnum;

        #region Constructor  ==================================================
        internal Enum_CompuType(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            Add(new PairZ(this, IdKey.CompuType_RowValue));
            Add(new PairZ(this, IdKey.CompuType_RelatedValue));
            Add(new PairZ(this, IdKey.CompuType_CompositeString));
            Add(new PairZ(this, IdKey.CompuType_CompositeReversed));
        }
        #endregion
    }
}
