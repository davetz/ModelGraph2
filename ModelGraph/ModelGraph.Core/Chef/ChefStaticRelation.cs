﻿using System;

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
        internal RelationOf<TableX, RelationX> TableX_ChildRelationX { get; private set; }
        internal RelationOf<TableX, RelationX> TableX_ParentRelationX { get; private set; }

        //private Relation Table_ChildRelationGroup;
        //private Relation Table_ParentRelationGroup;
        //private Relation Table_ReverseRelationGroup;
        //private Relation ChildRelationGroup_Relation;
        //private Relation ParentRelationGroup_Relation;
        //private Relation ReverseRelationGroup_Relation;

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
            Item_Error = new RelationOf<Item, Error>(RelationZStore, Trait.Item_Error, new Guid("3BD787B0-C38A-4288-9E12-1C688EEF984E"), Pairing.OneToMany, 25, 25);
            Store_Property = new RelationOf<Store, Property>(RelationZStore, Trait.Store_Property, Guid.Empty, Pairing.OneToMany, 40, 100, true);
            Store_ChildRelation = new RelationOf<Store, Relation>(RelationZStore, Trait.Store_ChildRelation, Guid.Empty, Pairing.ManyToMany, 25, 25, true);
            Store_ParentRelation = new RelationOf<Store, Relation>(RelationZStore, Trait.Store_ParentRelation, Guid.Empty, Pairing.ManyToMany, 25, 25, true);

            EnumX_ColumnX = new RelationOf<EnumX, ColumnX>(RelationStore, Trait.EnumX_ColumnX, new Guid("9179013E-57D5-4265-965B-1633B9252EDF"), Pairing.OneToMany, 25, 25);
            AddIntegrityCheck(EnumX_ColumnX, EnumXStore, ColumnXStore);

            TableX_ColumnX = new RelationOf<TableX,ColumnX>(RelationStore, Trait.TableX_ColumnX, new Guid("C6374551-5BC9-4906-B565-91A85D42FCEE"), Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(TableX_ColumnX, TableXStore, ColumnXStore);

            TableX_NameProperty = new RelationOf<TableX, Property>(RelationStore, Trait.TableX_NameProperty, new Guid("83717F0C-1C43-44A4-B3DF-7E81CDBCE638"), Pairing.OneToOne, 25, 25);
            AddIntegrityCheck(TableX_NameProperty, TableXStore, ColumnXStore);
            AddIntegrityCheck(TableX_NameProperty, TableXStore, ComputeXStore);

            TableX_SummaryProperty = new RelationOf<TableX, Property>(RelationStore, Trait.TableX_SummaryProperty, new Guid("332DBDC6-AAC2-4C8F-A192-D5780DE680F0"), Pairing.OneToOne, 25, 25);
            AddIntegrityCheck(TableX_SummaryProperty, TableXStore, ColumnXStore);
            AddIntegrityCheck(TableX_SummaryProperty, TableXStore, ComputeXStore);

            TableX_ChildRelationX = new RelationOf<TableX, RelationX>(RelationStore, Trait.TableX_ChildRelationX, new Guid("DDA99343-07F6-419B-9227-EB20EDC98982"), Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(TableX_ChildRelationX, TableXStore, RelationXStore);

            TableX_ParentRelationX = new RelationOf<TableX, RelationX>(RelationStore, Trait.TableX_ParentRelationX, new Guid("E38B8AA9-1F38-400E-8822-31E70DA1ECD7"), Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(TableX_ParentRelationX, TableXStore, RelationXStore);

            Store_ComputeX = new RelationOf<Store, ComputeX>(RelationStore, Trait.Store_ComputeX, new Guid("937964ED-21BB-429D-B829-4CC3FA3539FC"), Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(Store_ComputeX, EnumXStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, TableXStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, ColumnXStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, RelationXStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, ViewXStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, SymbolStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, ComputeXStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, PropertyStore, ComputeXStore);
            AddIntegrityCheck(Store_ComputeX, RelationStore, ComputeXStore);

            //Table_ChildRelationGroup = new Relation(_relationStore, Trait.TableChildRelationGroup, new Guid("E2A067DF-29B2-413A-B8F3-EEC2E46E0980"), Pairing.OneToMany, 25, 25, true);
            //Table_ParentRelationGroup = new Relation(_relationStore, Trait.TableParentRelationGroup, new Guid("5D785910-EEF3-4E2D-AE38-B7BC2EB87D42"), Pairing.OneToMany, 25, 25, true);
            //Table_ReverseRelationGroup = new Relation(_relationStore, Trait.TableReverseRelationGroup, new Guid("699D5034-EBDC-4AEA-9754-430BC6A72295"), Pairing.OneToMany, 25, 25, true);
            //ChildRelationGroup_Relation = new Relation(_relationStore, Trait.TableRelationGroupRelation, new Guid("DD96E791-920B-4C60-B245-A9C2F868E539"), Pairing.OneToMany, 25, 25);
            //ParentRelationGroup_Relation = new Relation(_relationStore, Trait.ParentRelationGroupRelation, new Guid("1892E827-042D-4700-99B2-25E504908EB7"), Pairing.OneToMany, 25, 25);
            //ReverseRelationGroup_Relation = new Relation(_relationStore, Trait.ReverseRelationGroupRelation, new Guid("0DCE9775-4944-4958-BE41-373401CCD564"), Pairing.OneToMany, 25, 25);

            ViewX_ViewX = new RelationOf<ViewX, ViewX>(RelationStore, Trait.ViewX_ViewX, new Guid("6CC10237-CD1D-4469-879E-020902ADA85C"), Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(ViewX_ViewX, ViewXStore, ViewXStore);

            QueryX_ViewX = new RelationOf<QueryX, ViewX>(RelationStore, Trait.QueryX_ViewX, new Guid("C5A5DBAD-5C95-4043-BC2B-75ED4C3F2B56"), Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(QueryX_ViewX, QueryXStore, ViewXStore);

            ViewX_QueryX = new RelationOf<ViewX, QueryX>(RelationStore, Trait.ViewX_QueryX, new Guid("80DF4DF6-4B8D-4D47-B7BE-C8EFF11CF8BB"), Pairing.OneToOne, 25, 25, true);
            AddIntegrityCheck(ViewX_QueryX, ViewXStore, QueryXStore);

            Property_ViewX = new RelationOf<Property, ViewX>(RelationStore, Trait.Property_ViewX, new Guid("3C73D6CC-1879-4450-BC8F-1D38F299617E"), Pairing.OneToMany, 25, 25);
            AddIntegrityCheck(Property_ViewX, ColumnXStore, ViewXStore);
            AddIntegrityCheck(Property_ViewX, ComputeXStore, ViewXStore);

            Relation_ViewX = new RelationOf<Relation, ViewX>(RelationStore, Trait.Relation_ViewX, new Guid("7FF00C7B-6EB2-40CF-A749-60E3E8E64AEF"), Pairing.OneToMany, 25, 25);
            AddIntegrityCheck(Relation_ViewX, RelationXStore, ViewXStore);


            GraphX_SymbolQueryX = new RelationOf<GraphX, QueryX>(RelationStore, Trait.GraphX_SymbolQueryX, new Guid("478BA83E-1EA7-48B1-9F18-8FC69B503215"), Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(GraphX_SymbolQueryX, GraphXStore, QueryXStore);

            GraphX_SymbolX = new RelationOf<GraphX, SymbolX>(RelationStore, Trait.GraphX_SymbolX, new Guid("82FA283D-7A07-452F-A5C8-0FF6E568D64D"), Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(GraphX_SymbolX, GraphXStore, SymbolStore);

            GraphX_ColorColumnX = new RelationOf<GraphX, ColumnX>(RelationStore, Trait.GraphX_ColorColumnX, new Guid("66C6D619-648A-4A7B-92E0-7224780819F0"), Pairing.OneToOne, 25, 25);
            AddIntegrityCheck(GraphX_ColorColumnX, GraphXStore, ColumnXStore);

            GraphX_QueryX = new RelationOf<GraphX, QueryX>(RelationStore, Trait.GraphX_QueryX, new Guid("B740311F-553A-4302-BDE3-53707B51DFCA"), Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(GraphX_QueryX, GraphXStore, QueryXStore);

            QueryX_QueryX = new RelationOf<QueryX, QueryX>(RelationStore, Trait.QueryX_QueryX, new Guid("41030BD7-4986-4CA0-B3E6-B194AFA75E14"), Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(QueryX_QueryX, QueryXStore, QueryXStore);
            AddIntegrityCheck(QueryX_QueryX, QueryXStore, QueryXStore);
            AddIntegrityCheck(QueryX_QueryX, QueryXStore, QueryXStore);

            QueryX_Property = new RelationOf<QueryX, Property>(RelationStore, Trait.QueryX_Property, new Guid("CE770665-173A-4B9C-8288-803FEFC0BC11"), Pairing.OneToMany, 25, 25);
            AddIntegrityCheck(QueryX_Property, QueryXStore, ColumnXStore);
            AddIntegrityCheck(QueryX_Property, QueryXStore, ComputeXStore);

            ViewX_Property = new RelationOf<ViewX, Property>(RelationStore, Trait.ViewX_Property, new Guid("87E57D4F-C9B8-46FD-930E-85E6DA77C169"), Pairing.OneToMany, 25, 25);
            AddIntegrityCheck(ViewX_Property, ViewXStore, ColumnXStore);
            AddIntegrityCheck(ViewX_Property, ViewXStore, ComputeXStore);

            Store_QueryX = new RelationOf<Store, QueryX>(RelationStore, Trait.Store_QueryX, new Guid("08A8840E-D218-4827-B442-F08DED3B82B8"), Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(Store_QueryX, EnumXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, ViewXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, TableXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, QueryXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, QueryXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, SymbolStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, ColumnXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, ComputeXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, ComputeXStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, PropertyStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, RelationStore, QueryXStore);
            AddIntegrityCheck(Store_QueryX, RelationXStore, QueryXStore);


            SymbolX_QueryX = new RelationOf<SymbolX, QueryX>(RelationStore, Trait.SymbolX_QueryX, new Guid("0D2836A0-A482-4E1F-8752-86A69BA5E5A6"), Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(SymbolX_QueryX, SymbolStore, QueryXStore);

            Relation_QueryX = new RelationOf<Relation, QueryX>(RelationStore, Trait.Relation_QueryX, new Guid("7F44E7A0-CEEF-4751-A377-C6B01BC1E06E"), Pairing.OneToMany, 25, 25, true);
            AddIntegrityCheck(Relation_QueryX, RelationXStore, QueryXStore);
            AddIntegrityCheck(Relation_QueryX, RelationXStore, QueryXStore);

            AddIntegrityCheck(Relation_QueryX, RelationStore, QueryXStore);
            AddIntegrityCheck(Relation_QueryX, RelationStore, QueryXStore);


            ComputeX_QueryX = new RelationOf<ComputeX, QueryX>(RelationStore, Trait.ComputeX_QueryX, new Guid("3EFADD1B-28FF-4A84-90B1-A7948DB7FBBD"), Pairing.OneToOne, 25, 25, true);
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

        #region ReleaseRelations  =============================================
        private void ReleaseRelations()
        {
            Item_Error  = null;
            Store_Property  = null;
            Store_ChildRelation  = null;
            Store_ParentRelation  = null;

            EnumX_ColumnX  = null;

            TableX_ColumnX  = null;

            TableX_NameProperty  = null;

            TableX_SummaryProperty  = null;

            TableX_ChildRelationX  = null;

            TableX_ParentRelationX  = null;

            Store_ComputeX  = null;
            ViewX_ViewX  = null;

            QueryX_ViewX  = null;

            ViewX_QueryX  = null;

            Property_ViewX  = null;

            Relation_ViewX  = null;


            GraphX_SymbolQueryX  = null;

            GraphX_SymbolX  = null;

            GraphX_ColorColumnX  = null;

            GraphX_QueryX  = null;

            QueryX_QueryX  = null;

            QueryX_Property  = null;

            ViewX_Property  = null;

            Store_QueryX  = null;


            SymbolX_QueryX  = null;

            Relation_QueryX  = null;

            ComputeX_QueryX  = null;
        }
        #endregion

    }
}
