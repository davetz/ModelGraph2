
namespace ModelGraph.Core
{/*
 */
    public class Enum_SideType : EnumZ
    {
        internal override IdKey ViKey => IdKey.SideEnum;

        #region Constructor  ==================================================
        internal Enum_SideType(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            Add(new PairZ(this, IdKey.Side_Any));
            Add(new PairZ(this, IdKey.Side_East));
            Add(new PairZ(this, IdKey.Side_West));
            Add(new PairZ(this, IdKey.Side_North));
            Add(new PairZ(this, IdKey.Side_South));
        }
        #endregion
    }
}
