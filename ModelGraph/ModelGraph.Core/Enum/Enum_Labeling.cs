
namespace ModelGraph.Core
{/*
 */
    public class Enum_Labeling : EnumZ
    {
        internal override IdKey IdKey => IdKey.LabelingEnum;

        #region Constructor  ==================================================
        internal Enum_Labeling(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            Add(new PairZ(this, IdKey.Labeling_None));
            Add(new PairZ(this, IdKey.Labeling_Top));
            Add(new PairZ(this, IdKey.Labeling_Left));
            Add(new PairZ(this, IdKey.Labeling_Right));
            Add(new PairZ(this, IdKey.Labeling_Bottom));
            Add(new PairZ(this, IdKey.Labeling_Center));
            Add(new PairZ(this, IdKey.Labeling_TopLeft));
            Add(new PairZ(this, IdKey.Labeling_TopRight));
            Add(new PairZ(this, IdKey.Labeling_BottomLeft));
            Add(new PairZ(this, IdKey.Labeling_BottomRight));
            Add(new PairZ(this, IdKey.Labeling_TopLeftSide));
            Add(new PairZ(this, IdKey.Labeling_TopRightSide));
            Add(new PairZ(this, IdKey.Labeling_TopLeftCorner));
            Add(new PairZ(this, IdKey.Labeling_TopRightCorner));
            Add(new PairZ(this, IdKey.Labeling_BottomLeftSide));
            Add(new PairZ(this, IdKey.Labeling_BottomRightSide));
            Add(new PairZ(this, IdKey.Labeling_BottomLeftCorner));
            Add(new PairZ(this, IdKey.Labeling_BottomRightCorner));
        }
        #endregion
    }
}
