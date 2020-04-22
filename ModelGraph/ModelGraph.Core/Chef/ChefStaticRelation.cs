using System;

namespace ModelGraph.Core
{
    public partial class Chef
    {
        internal RelationOf<Store, Property> Store_Property { get; private set; }
        internal RelationOf<Store, QueryX> Store_QueryX { get; private set; }
        internal RelationOf<Store, ColumnX> Store_ColumnX { get; private set; }
        internal RelationOf<Store, ComputeX> Store_ComputeX { get; private set; }
        internal RelationOf<Store, Property> Store_NameProperty { get; private set; } // can be a ColumnX or a ComputeX
        internal RelationOf<Store, Property> Store_SummaryProperty { get; private set; } // can be a ColumnX or a ComputeX
        internal RelationOf<Store, Relation> Store_ChildRelation { get; private set; }
        internal RelationOf<Store, Relation> Store_ParentRelation { get; private set; }


        internal RelationOf<Item, Error> Item_Error { get; private set; }
        internal RelationOf<ViewX, ViewX> ViewX_ViewX { get; private set; }
        internal RelationOf<QueryX, ViewX> QueryX_ViewX { get; private set; }
        internal RelationOf<Property, ViewX> Property_ViewX { get; private set; }
        internal RelationOf<Relation, ViewX>  Relation_ViewX { get; private set; }

        internal RelationOf<GraphX, QueryX> GraphX_SymbolQueryX { get; private set; } //REDUNDANT ??

        internal RelationOf<GraphX, ColumnX>  GraphX_ColorColumnX { get; private set; }
        internal RelationOf<GraphX, SymbolX>   GraphX_SymbolX { get; private set; }

        internal RelationOf<ViewX, QueryX> ViewX_QueryX { get; private set; }
        internal RelationOf<GraphX, QueryX> GraphX_QueryX { get; private set; }
        internal RelationOf<QueryX, QueryX> QueryX_QueryX { get; private set; }
        internal RelationOf<SymbolX, QueryX> SymbolX_QueryX { get; private set; }
        internal RelationOf<ComputeX, QueryX> ComputeX_QueryX { get; private set; }
        internal RelationOf<Relation, QueryX> Relation_QueryX { get; private set; }

        internal RelationOf<ViewX, Property> ViewX_Property { get; private set; }
        internal RelationOf<QueryX, Property> QueryX_Property { get; private set; }

        internal RelationOf<EnumX, ColumnX> EnumX_ColumnX { get; private set; }

        #region InitializeRelations  ==========================================
        private void InitializeRelations()
        {
            Item_Error = new RelationOf<Item, Error>(RelationZStore, IdKey.Item_Error, Pairing.OneToMany, 25, 25);
            Store_Property = new RelationOf<Store, Property>(RelationZStore, IdKey.Store_Property, Pairing.OneToMany, 40, 100, true);
            Store_ChildRelation = new RelationOf<Store, Relation>(RelationStore, IdKey.Store_ChildRelation, Pairing.OneToMany, 25, 25, true);
            Store_ParentRelation = new RelationOf<Store, Relation>(RelationStore, IdKey.Store_ParentRelation, Pairing.OneToMany, 25, 25, true);

            EnumX_ColumnX = new RelationOf<EnumX, ColumnX>(RelationStore, IdKey.EnumX_ColumnX, Pairing.OneToMany, 25, 25);

            Store_ColumnX = new RelationOf<Store,ColumnX>(RelationStore, IdKey.Store_ColumnX, Pairing.OneToMany, 25, 25, true);

            Store_NameProperty = new RelationOf<Store, Property>(RelationStore, IdKey.Store_NameProperty, Pairing.OneToOne, 25, 25);

            Store_SummaryProperty = new RelationOf<Store, Property>(RelationStore, IdKey.Store_SummaryProperty, Pairing.OneToOne, 25, 25);

            Store_ComputeX = new RelationOf<Store, ComputeX>(RelationStore, IdKey.Store_ComputeX, Pairing.OneToMany, 25, 25, true);

            ViewX_ViewX = new RelationOf<ViewX, ViewX>(RelationStore, IdKey.ViewX_ViewX, Pairing.OneToMany, 25, 25, true);

            QueryX_ViewX = new RelationOf<QueryX, ViewX>(RelationStore, IdKey.QueryX_ViewX, Pairing.OneToMany, 25, 25, true);

            ViewX_QueryX = new RelationOf<ViewX, QueryX>(RelationStore, IdKey.ViewX_QueryX, Pairing.OneToOne, 25, 25, true);

            Property_ViewX = new RelationOf<Property, ViewX>(RelationStore, IdKey.Property_ViewX, Pairing.OneToMany, 25, 25);

            Relation_ViewX = new RelationOf<Relation, ViewX>(RelationStore, IdKey.Relation_ViewX, Pairing.OneToMany, 25, 25);

            GraphX_SymbolQueryX = new RelationOf<GraphX, QueryX>(RelationStore, IdKey.GraphX_SymbolQueryX, Pairing.OneToMany, 25, 25, true);

            GraphX_SymbolX = new RelationOf<GraphX, SymbolX>(RelationStore, IdKey.GraphX_SymbolX, Pairing.OneToMany, 25, 25, true);

            GraphX_ColorColumnX = new RelationOf<GraphX, ColumnX>(RelationStore, IdKey.GraphX_ColorColumnX, Pairing.OneToOne, 25, 25);

            GraphX_QueryX = new RelationOf<GraphX, QueryX>(RelationStore, IdKey.GraphX_QueryX, Pairing.OneToMany, 25, 25, true);

            QueryX_QueryX = new RelationOf<QueryX, QueryX>(RelationStore, IdKey.QueryX_QueryX, Pairing.OneToMany, 25, 25, true);

            QueryX_Property = new RelationOf<QueryX, Property>(RelationStore, IdKey.QueryX_Property, Pairing.OneToMany, 25, 25);

            ViewX_Property = new RelationOf<ViewX, Property>(RelationStore, IdKey.ViewX_Property, Pairing.OneToMany, 25, 25);

            Store_QueryX = new RelationOf<Store, QueryX>(RelationStore, IdKey.Store_QueryX, Pairing.OneToMany, 25, 25, true);

            SymbolX_QueryX = new RelationOf<SymbolX, QueryX>(RelationStore, IdKey.SymbolX_QueryX, Pairing.OneToMany, 25, 25, true);

            Relation_QueryX = new RelationOf<Relation, QueryX>(RelationStore, IdKey.Relation_QueryX, Pairing.OneToMany, 25, 25, true);

            ComputeX_QueryX = new RelationOf<ComputeX, QueryX>(RelationStore, IdKey.ComputeX_QueryX, Pairing.OneToOne, 25, 25, true);
        }
        #endregion
    }
}
