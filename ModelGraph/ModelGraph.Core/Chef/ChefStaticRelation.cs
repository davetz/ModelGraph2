using System;

namespace ModelGraph.Core
{
    public partial class Chef
    {
        internal RelationOf<Store, Property> Store_Property { get; private set; }
        internal RelationOf<Store, Relation> Store_ChildRelation { get; private set; }
        internal RelationOf<Store, Relation> Store_ParentRelation { get; private set; }

        internal RelationOf<TableX, ColumnX> TableX_ColumnX { get; private set; }
        internal RelationOf<TableX, Property> TableX_NameProperty { get; private set; } // can be a ColumnX or a ComputeX
        internal RelationOf<TableX, Property> TableX_SummaryProperty { get; private set; } // can be a ColumnX or a ComputeX
        internal RelationOf<TableX, RelationXO> TableX_ChildRelationX { get; private set; }
        internal RelationOf<TableX, RelationXO> TableX_ParentRelationX { get; private set; }

        internal RelationOf<Item, Error> Item_Error { get; private set; }
        internal RelationOf<ViewX, ViewX> ViewX_ViewX { get; private set; }
        internal RelationOf<QueryX, ViewX> QueryX_ViewX { get; private set; }
        internal RelationOf<Property, ViewX> Property_ViewX { get; private set; }
        internal RelationOf<Relation, ViewX>  Relation_ViewX { get; private set; }

        internal RelationOf<GraphX, QueryX> GraphX_SymbolQueryX { get; private set; } //REDUNDANT ??

        internal RelationOf<GraphX, ColumnX>  GraphX_ColorColumnX { get; private set; }
        internal RelationOf<GraphX, SymbolX>   GraphX_SymbolX { get; private set; }

        internal RelationOf<ViewX, QueryX> ViewX_QueryX { get; private set; }
        internal RelationOf<Store, QueryX> Store_QueryX { get; private set; }
        internal RelationOf<GraphX, QueryX> GraphX_QueryX { get; private set; }
        internal RelationOf<QueryX, QueryX> QueryX_QueryX { get; private set; }
        internal RelationOf<SymbolX, QueryX> SymbolX_QueryX { get; private set; }
        internal RelationOf<ComputeX, QueryX> ComputeX_QueryX { get; private set; }
        internal RelationOf<Relation, QueryX> Relation_QueryX { get; private set; }

        internal RelationOf<ViewX, Property> ViewX_Property { get; private set; }
        internal RelationOf<QueryX, Property> QueryX_Property { get; private set; }

        internal RelationOf<EnumX, ColumnX> EnumX_ColumnX { get; private set; }
        internal RelationOf<Store, ComputeX>   Store_ComputeX { get; private set; }

        #region InitializeRelations  ==========================================
        private void InitializeRelations()
        {
            Item_Error = new RelationOf<Item, Error>(RelationZStore, IdKey.Item_Error, Pairing.OneToMany, 25, 25);
            Store_Property = new RelationOf<Store, Property>(RelationZStore, IdKey.Store_Property, Pairing.OneToMany, 40, 100, true);
            Store_ChildRelation = new RelationOf<Store, Relation>(RelationZStore, IdKey.Store_ChildRelation, Pairing.ManyToMany, 25, 25, true);
            Store_ParentRelation = new RelationOf<Store, Relation>(RelationZStore, IdKey.Store_ParentRelation, Pairing.ManyToMany, 25, 25, true);

            EnumX_ColumnX = new RelationOf<EnumX, ColumnX>(RelationStore, IdKey.EnumX_ColumnX, Pairing.OneToMany, 25, 25);
            AddIntegrityCheck(EnumX_ColumnX, EnumXStore, ColumnXStore);

            TableX_ColumnX = new RelationOf<TableX,ColumnX>(RelationStore, IdKey.TableX_ColumnX, Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(TableX_ColumnX, TableXStore, ColumnXStore);

            TableX_NameProperty = new RelationOf<TableX, Property>(RelationStore, IdKey.TableX_NameProperty, Pairing.OneToOne, 25, 25);
            AddIntegrityCheck(TableX_NameProperty, TableXStore, ColumnXStore);
            AddIntegrityCheck(TableX_NameProperty, TableXStore, ComputeXStore);

            TableX_SummaryProperty = new RelationOf<TableX, Property>(RelationStore, IdKey.TableX_SummaryProperty, Pairing.OneToOne, 25, 25);
            AddIntegrityCheck(TableX_SummaryProperty, TableXStore, ColumnXStore);
            AddIntegrityCheck(TableX_SummaryProperty, TableXStore, ComputeXStore);

            TableX_ChildRelationX = new RelationOf<TableX, RelationXO>(RelationStore, IdKey.TableX_ChildRelationX, Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(TableX_ChildRelationX, TableXStore, RelationXStore);

            TableX_ParentRelationX = new RelationOf<TableX, RelationXO>(RelationStore, IdKey.TableX_ParentRelationX, Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(TableX_ParentRelationX, TableXStore, RelationXStore);

            Store_ComputeX = new RelationOf<Store, ComputeX>(RelationStore, IdKey.Store_ComputeX, Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(Store_ComputeX, EnumXStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, TableXStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, ColumnXStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, RelationXStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, ViewXStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, SymbolXStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, ComputeXStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, PropertyStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, RelationStore, ComputeXStore);

            //Table_ChildRelationGroup = new Relation(_relationStore, IdKey.TableChildRelationGroup, Pairing.OneToMany, 25, 25, true);
            //Table_ParentRelationGroup = new Relation(_relationStore, IdKey.TableParentRelationGroup, Pairing.OneToMany, 25, 25, true);
            //Table_ReverseRelationGroup = new Relation(_relationStore, IdKey.TableReverseRelationGroup, Pairing.OneToMany, 25, 25, true);
            //ChildRelationGroup_Relation = new Relation(_relationStore, IdKey.TableRelationGroupRelation, Pairing.OneToMany, 25, 25);
            //ParentRelationGroup_Relation = new Relation(_relationStore, IdKey.ParentRelationGroupRelation, Pairing.OneToMany, 25, 25);
            //ReverseRelationGroup_Relation = new Relation(_relationStore, IdKey.ReverseRelationGroupRelation, Pairing.OneToMany, 25, 25);

            ViewX_ViewX = new RelationOf<ViewX, ViewX>(RelationStore, IdKey.ViewX_ViewX, Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(ViewX_ViewX, ViewXStore, ViewXStore);

            QueryX_ViewX = new RelationOf<QueryX, ViewX>(RelationStore, IdKey.QueryX_ViewX, Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(QueryX_ViewX, QueryXStore, ViewXStore);

            ViewX_QueryX = new RelationOf<ViewX, QueryX>(RelationStore, IdKey.ViewX_QueryX, Pairing.OneToOne, 25, 25, true);
            AddIntegrityCheck(ViewX_QueryX, ViewXStore, QueryXStore);

            Property_ViewX = new RelationOf<Property, ViewX>(RelationStore, IdKey.Property_ViewX, Pairing.OneToMany, 25, 25);
            AddIntegrityCheck(Property_ViewX, ColumnXStore, ViewXStore);
            AddIntegrityCheck(Property_ViewX, ComputeXStore, ViewXStore);

            Relation_ViewX = new RelationOf<Relation, ViewX>(RelationStore, IdKey.Relation_ViewX, Pairing.OneToMany, 25, 25);
            AddIntegrityCheck(Relation_ViewX, RelationXStore, ViewXStore);


            GraphX_SymbolQueryX = new RelationOf<GraphX, QueryX>(RelationStore, IdKey.GraphX_SymbolQueryX, Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(GraphX_SymbolQueryX, GraphXStore, QueryXStore);

            GraphX_SymbolX = new RelationOf<GraphX, SymbolX>(RelationStore, IdKey.GraphX_SymbolX, Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(GraphX_SymbolX, GraphXStore, SymbolXStore);

            GraphX_ColorColumnX = new RelationOf<GraphX, ColumnX>(RelationStore, IdKey.GraphX_ColorColumnX, Pairing.OneToOne, 25, 25);
            AddIntegrityCheck(GraphX_ColorColumnX, GraphXStore, ColumnXStore);

            GraphX_QueryX = new RelationOf<GraphX, QueryX>(RelationStore, IdKey.GraphX_QueryX, Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(GraphX_QueryX, GraphXStore, QueryXStore);

            QueryX_QueryX = new RelationOf<QueryX, QueryX>(RelationStore, IdKey.QueryX_QueryX, Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(QueryX_QueryX, QueryXStore, QueryXStore);
            AddIntegrityCheck(QueryX_QueryX, QueryXStore, QueryXStore);
            AddIntegrityCheck(QueryX_QueryX, QueryXStore, QueryXStore);

            QueryX_Property = new RelationOf<QueryX, Property>(RelationStore, IdKey.QueryX_Property, Pairing.OneToMany, 25, 25);
            AddIntegrityCheck(QueryX_Property, QueryXStore, ColumnXStore);
            AddIntegrityCheck(QueryX_Property, QueryXStore, ComputeXStore);

            ViewX_Property = new RelationOf<ViewX, Property>(RelationStore, IdKey.ViewX_Property, Pairing.OneToMany, 25, 25);
            AddIntegrityCheck(ViewX_Property, ViewXStore, ColumnXStore);
            AddIntegrityCheck(ViewX_Property, ViewXStore, ComputeXStore);

            Store_QueryX = new RelationOf<Store, QueryX>(RelationStore, IdKey.Store_QueryX, Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(Store_QueryX, EnumXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, ViewXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, TableXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, QueryXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, QueryXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, SymbolXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, ColumnXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, ComputeXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, ComputeXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, PropertyStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, RelationStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, RelationXStore, QueryXStore);


            SymbolX_QueryX = new RelationOf<SymbolX, QueryX>(RelationStore, IdKey.SymbolX_QueryX, Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(SymbolX_QueryX, SymbolXStore, QueryXStore);

            Relation_QueryX = new RelationOf<Relation, QueryX>(RelationStore, IdKey.Relation_QueryX, Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(Relation_QueryX, RelationXStore, QueryXStore);
            AddIntegrityCheck(Relation_QueryX, RelationXStore, QueryXStore);

            AddIntegrityCheck(Relation_QueryX, RelationStore, QueryXStore);
            AddIntegrityCheck(Relation_QueryX, RelationStore, QueryXStore);


            ComputeX_QueryX = new RelationOf<ComputeX, QueryX>(RelationStore, IdKey.ComputeX_QueryX, Pairing.OneToOne, 25, 25, true);
            AddIntegrityCheck(ComputeX_QueryX, ComputeXStore, QueryXStore);
        }
        /// <summary>
        /// Build dependency dictionaries used for maintaining relational integrity
        /// </summary>
        private void AddIntegrityCheck(Relation relation, Store parentStore, Store childStore)
        {
            if (!Store_ChildRelation.ContainsLink(parentStore, relation))
                Store_ChildRelation.SetLink(parentStore, relation);

            if (!Store_ParentRelation.ContainsLink(childStore, relation))
                Store_ParentRelation.SetLink(childStore, relation);
        }
        #endregion
    }
}
