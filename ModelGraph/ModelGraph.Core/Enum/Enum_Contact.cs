
namespace ModelGraph.Core
{
    public class Enum_Contact : EnumZ
    {
        internal override IdKey ViKey => IdKey.ContactEnum;

        #region Constructor  ==================================================
        internal Enum_Contact(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            Add(new PairZ(this, IdKey.Contact_Any));
            Add(new PairZ(this, IdKey.Contact_One));
            Add(new PairZ(this, IdKey.Contact_None));
        }
        #endregion
    }
}
