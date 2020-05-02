
namespace ModelGraph.Core
{/*
 */
    public class Enum_Connect : EnumZ
    {
        internal override IdKey ViKey => IdKey.ConnectEnum;

        #region Constructor  ==================================================
        internal Enum_Connect(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            Add(new PairZ(this, IdKey.Connect_Any));
            Add(new PairZ(this, IdKey.Connect_East));
            Add(new PairZ(this, IdKey.Connect_West));
            Add(new PairZ(this, IdKey.Connect_North));
            Add(new PairZ(this, IdKey.Connect_South));
            Add(new PairZ(this, IdKey.Connect_East_West));
            Add(new PairZ(this, IdKey.Connect_North_South));
            Add(new PairZ(this, IdKey.Connect_North_East));
            Add(new PairZ(this, IdKey.Connect_North_West));
            Add(new PairZ(this, IdKey.Connect_North_East_West));
            Add(new PairZ(this, IdKey.Connect_North_South_East));
            Add(new PairZ(this, IdKey.Connect_North_South_West));
            Add(new PairZ(this, IdKey.Connect_South_East));
            Add(new PairZ(this, IdKey.Connect_South_West));
            Add(new PairZ(this, IdKey.Connect_South_East_West));
        }
        #endregion
    }
}
