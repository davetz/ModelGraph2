﻿
namespace ModelGraph.Core
{
    public class PropertyRoot : InternalRoot<Property>
    {
        internal PropertyRoot(Root root)
        {
            Owner = root;

            SetCapacity(100);
            CreateProperties(root);
        }

        #region CreateProperties  =============================================
        private void CreateProperties(Root root)
        {
            root.RegisterReferenceItem(new Property_Item_Name(this)); 
            root.RegisterReferenceItem(new Property_Item_Summary(this));
            root.RegisterReferenceItem(new Property_Item_Description(this));
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.PropertyRoot;
        public override string GetParentNameId(Root root) => GetKindId(root);
        #endregion

    }
}