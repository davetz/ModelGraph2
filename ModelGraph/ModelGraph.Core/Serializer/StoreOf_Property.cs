
namespace ModelGraph.Core
{
    public class StoreOf_Property : StoreOf_Internal<Property>
    {
        internal StoreOf_Property(Chef chef)
        {
            Owner = chef;

            SetCapacity(100);
            CreateProperties(chef);

            chef.Add(this);
        }

        #region CreateProperties  =============================================
        private void CreateProperties(Chef chef)
        {
            chef.RegisterReferenceItem(new Property_Item_Name(this)); 
            chef.RegisterReferenceItem(new Property_Item_Summary(this));
            chef.RegisterReferenceItem(new Property_Item_Description(this));
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.PropertyDomain;
        public override string GetParentNameId(Chef chef) => GetKindId(chef);
        #endregion

    }
}
