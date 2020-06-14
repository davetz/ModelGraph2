using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class RelationRoot : InternalRoot<Relation>, IRelationRoot
    {
        internal RelationRoot(Root root)
        {
            Owner = root;

            SetCapacity(30);
            new RelationLink(root, this);
            CreateRelations(root);

            root.Add(this);
        }

        #region CreateRelations  ==============================================
        private void CreateRelations(Root root)
        {
            root.RegisterPrivateItem(new Relation_Item_Error(this));

            root.RegisterReferenceItem(new Relation_ComputeX_QueryX(this));
            root.RegisterReferenceItem(new Relation_EnumX_ColumnX(this));
            root.RegisterReferenceItem(new Relation_GraphX_ColorColumnX(this));
            root.RegisterReferenceItem(new Relation_GraphX_QueryX(this));
            root.RegisterReferenceItem(new Relation_GraphX_SymbolQueryX(this));
            root.RegisterReferenceItem(new Relation_GraphX_SymbolX(this));
            root.RegisterReferenceItem(new Relation_Property_ViewX(this));
            root.RegisterReferenceItem(new Relation_QueryX_Property(this));
            root.RegisterReferenceItem(new Relation_QueryX_QueryX(this));
            root.RegisterReferenceItem(new Relation_QueryX_ViewX(this));
            root.RegisterReferenceItem(new Relation_Relation_QueryX(this));
            root.RegisterReferenceItem(new Relation_Relation_ViewX(this));
            root.RegisterReferenceItem(new Relation_Store_ChildRelation(this));
            root.RegisterReferenceItem(new Relation_Store_ColumnX(this));
            root.RegisterReferenceItem(new Relation_Store_ComputeX(this));
            root.RegisterReferenceItem(new Relation_Store_NameProperty(this));
            root.RegisterReferenceItem(new Relation_Store_ParentRelation(this));
            root.RegisterReferenceItem(new Relation_Store_QueryX(this));
            root.RegisterReferenceItem(new Relation_Store_SummaryProperty(this));
            root.RegisterReferenceItem(new Relation_SymbolX_QueryX(this));
            root.RegisterReferenceItem(new Relation_ViewX_Property(this));
            root.RegisterReferenceItem(new Relation_ViewX_QueryX(this));
            root.RegisterReferenceItem(new Relation_ViewX_ViewX(this));
        }
        #endregion

        #region GetRelationArray  =============================================
        public Relation[] GetRelationArray()
        {
            var relationArray = new Relation[Count];
            for (int i = 0; i < Count; i++)
            {
                relationArray[i] = Items[i];
            }
            return relationArray;
        }
        #endregion

        #region Identity  =====================================================
        internal override IdKey IdKey => IdKey.RelationRoot;
        public override string GetParentNameId(Root root) => GetKindId(root);
        #endregion
    }
}
