using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class StoreOf_Relation : StoreOf_Internal<Relation>, IRelationStore
    {
        internal StoreOf_Relation(Chef chef)
        {
            Owner = chef;

            SetCapacity(30);
            new RelationLink(chef, this);
            CreateRelations(chef);

            chef.Add(this);
        }

        #region CreateRelations  ==============================================
        private void CreateRelations(Chef chef)
        {
            var priv = chef.Get<StoreOf_RelationZ>();

            chef.RegisterPrivateItem(new Relation_Item_Error(priv));

            chef.RegisterReferenceItem(new Relation_ComputeX_QueryX(this));
            chef.RegisterReferenceItem(new Relation_EnumX_ColumnX(this));
            chef.RegisterReferenceItem(new Relation_GraphX_ColorColumnX(this));
            chef.RegisterReferenceItem(new Relation_GraphX_QueryX(this));
            chef.RegisterReferenceItem(new Relation_GraphX_SymbolQueryX(this));
            chef.RegisterReferenceItem(new Relation_GraphX_SymbolX(this));
            chef.RegisterReferenceItem(new Relation_Property_ViewX(this));
            chef.RegisterReferenceItem(new Relation_QueryX_Property(this));
            chef.RegisterReferenceItem(new Relation_QueryX_QueryX(this));
            chef.RegisterReferenceItem(new Relation_QueryX_ViewX(this));
            chef.RegisterReferenceItem(new Relation_Relation_QueryX(this));
            chef.RegisterReferenceItem(new Relation_Relation_ViewX(this));
            chef.RegisterReferenceItem(new Relation_Store_ChildRelation(this));
            chef.RegisterReferenceItem(new Relation_Store_ColumnX(this));
            chef.RegisterReferenceItem(new Relation_Store_ComputeX(this));
            chef.RegisterReferenceItem(new Relation_Store_NameProperty(this));
            chef.RegisterReferenceItem(new Relation_Store_ParentRelation(this));
            chef.RegisterReferenceItem(new Relation_Store_QueryX(this));
            chef.RegisterReferenceItem(new Relation_Store_SummaryProperty(this));
            chef.RegisterReferenceItem(new Relation_SymbolX_QueryX(this));
            chef.RegisterReferenceItem(new Relation_ViewX_Property(this));
            chef.RegisterReferenceItem(new Relation_ViewX_QueryX(this));
            chef.RegisterReferenceItem(new Relation_ViewX_ViewX(this));
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
        internal override IdKey IdKey => IdKey.RelationDomain;
        public override string GetParentNameId(Chef chef) => GetKindId(chef);
        #endregion
    }
}
