
namespace ModelGraph.Core
{/*
 */
    public class Enum_Facet : EnumZ
    {
        internal override IdKey ViKey => IdKey.FacetEnum;

        #region Constructor  ==================================================
        internal Enum_Facet(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            Add(new PairZ(this, IdKey.Facet_None));
            Add(new PairZ(this, IdKey.Facet_Nubby));
            Add(new PairZ(this, IdKey.Facet_Diamond));
            Add(new PairZ(this, IdKey.Facet_InArrow));
            Add(new PairZ(this, IdKey.Facet_Force_None));
            Add(new PairZ(this, IdKey.Facet_Force_Nubby));
            Add(new PairZ(this, IdKey.Facet_Force_Diamond));
            Add(new PairZ(this, IdKey.Facet_Force_InArrow));
        }
        #endregion
    }
}
