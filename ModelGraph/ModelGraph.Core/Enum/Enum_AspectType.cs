
namespace ModelGraph.Core
{/*
 */
    public class Enum_AspectType : EnumZ
    {
        internal override IdKey ViKey => IdKey.AspectEnum;

        #region Constructor  ==================================================
        internal Enum_AspectType(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            Add(new PairZ(this, IdKey.Aspect_Point));
            Add(new PairZ(this, IdKey.Aspect_Square));
            Add(new PairZ(this, IdKey.Aspect_Vertical));
            Add(new PairZ(this, IdKey.Aspect_Horizontal));
        }
        #endregion
    }
}
