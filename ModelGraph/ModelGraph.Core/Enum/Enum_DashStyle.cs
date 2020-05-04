
namespace ModelGraph.Core
{
    public class Enum_DashStyle : EnumZ
    {
        internal override IdKey IdKey => IdKey.DashStyleEnum;

        #region Constructor  ==================================================
        internal Enum_DashStyle(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            Add(new PairZ(this, IdKey.DashStyle_Solid));
            Add(new PairZ(this, IdKey.DashStyle_Dashed));
            Add(new PairZ(this, IdKey.DashStyle_Dotted));
            Add(new PairZ(this, IdKey.DashStyle_DashDot));
            Add(new PairZ(this, IdKey.DashStyle_DashDotDot));
        }
        #endregion
    }
}
