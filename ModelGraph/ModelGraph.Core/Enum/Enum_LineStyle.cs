
namespace ModelGraph.Core
{
    public class Enum_LineStyle : EnumZ
    {
        internal override IdKey IdKey => IdKey.LineStyleEnum;

        #region Constructor  ==================================================
        internal Enum_LineStyle(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            Add(new PairZ(this, IdKey.LineStyle_PointToPoint));
            Add(new PairZ(this, IdKey.LineStyle_SimpleSpline));
            Add(new PairZ(this, IdKey.LineStyle_DoubleSpline));
        }
        #endregion
    }
}
