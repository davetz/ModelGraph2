
namespace ModelGraph.Core
{/*
 */
    public class Enum_Aspect : EnumZ
    {
        internal override IdKey ViKey => IdKey.AspectEnum;

        #region Constructor  ==================================================
        internal Enum_Aspect(StoreOf<EnumZ> owner)
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
