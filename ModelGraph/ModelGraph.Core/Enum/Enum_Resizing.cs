
namespace ModelGraph.Core
{/*
 */
    public class Enum_Resizing : EnumZ
    {
        internal override IdKey ViKey => IdKey.ResizingEnum;

        #region Constructor  ==================================================
        internal Enum_Resizing(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            Add(new PairZ(this, IdKey.Resizing_Auto));
            Add(new PairZ(this, IdKey.Resizing_Fixed));
            Add(new PairZ(this, IdKey.Resizing_Manual));
        }
        #endregion
    }
}
