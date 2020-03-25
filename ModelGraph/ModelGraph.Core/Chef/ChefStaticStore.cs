using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public partial class Chef
    {
        internal static IList<Item> EmptyItemList = new List<Item>(0).AsReadOnly();

        internal InternalStore<Item> DummyStore;
        internal PrivateStore<Store> PrivateStores;
        internal PrivateStore<Store> InternalStores;
        internal PrivateStore<Store> ExternalStores;

        internal PrivateStore<Error> ErrorStore { get; private set; }
        internal PrivateStore<EnumZ> EnumZStore { get; private set; }
        internal PrivateStore<Property> PropertyZStore { get; private set; }
        internal PrivateStore<Relation> RelationZStore { get; private set; }

        internal InternalStore<Property> PropertyStore { get; private set; }
        internal InternalStore<Relation> RelationStore { get; private set; }

        internal Dummy Dummy { get; private set; }

        internal QueryX QueryXNode { get; private set; }

        internal ChangeSet ChangeSet { get; private set; }

        internal ChangeRoot ChangeRoot { get; private set; }

        internal int ChangeSequence { get; private set; }

        internal Store[] PrimeStores { get; private set; }
        internal EnumXStore EnumXStore { get; private set; }

        internal ViewXStore ViewXStore { get; private set; }

        internal StoreOf<TableX> TableXStore { get; private set; }

        internal StoreOf<GraphX> GraphXStore { get; private set; }

        internal StoreOf<QueryX> QueryXStore { get; private set; }

        internal StoreOf<ColumnX> ColumnXStore { get; private set; }

        internal StoreOf<SymbolX> SymbolXStore { get; private set; }

        internal StoreOf<ComputeX> ComputeXStore { get; private set; }

        internal RelationXStore RelationXStore { get; private set; }





        #region InitializeStores  =============================================
        private void InitializeStores()
        {
            PrivateStores = new PrivateStore<Store>(this, Trait.PrivateStores, 20);
            Add(PrivateStores);

            InternalStores = new PrivateStore<Store>(this, Trait.InternalStores, 20);
            Add(InternalStores);

            ExternalStores = new PrivateStore<Store>(this, Trait.ExternalStores, 20);
            Add(ExternalStores);

            DummyStore = new InternalStore<Item>(this, Trait.DummyStore, 1);
            Dummy = new Dummy(this);

            QueryXNode = new QueryX(this);
            ChangeRoot = new ChangeRoot(this);
            ChangeSequence = 1;
            ChangeSet = new ChangeSet(ChangeRoot, ChangeSequence);

            ErrorStore = new PrivateStore<Error>(this, Trait.ErrorStore, 20);
            EnumZStore = new PrivateStore<EnumZ>(this, Trait.EnumZStore, 20);
            PropertyZStore = new PrivateStore<Property>(this, Trait.PropertyZStore, 10);
            RelationZStore = new PrivateStore<Relation>(this, Trait.RelationZStore, 10);

            PropertyStore = new InternalStore<Property>(this, Trait.PropertyStore, 100);
            RelationStore = new InternalStore<Relation>(this, Trait.RelationStore, 100);

            EnumXStore = new EnumXStore(this);
            ViewXStore = new ViewXStore(this);
            TableXStore = new TableXStore(this);
            GraphXStore = new StoreOf<GraphX>(this, Trait.GraphXStore, new System.Guid("72C2BEC8-B8C8-44A1-ADF0-3832416820F3"), 30);
            QueryXStore = new StoreOf<QueryX>(this, Trait.QueryXStore, new System.Guid("085A1887-03FE-4DA1-9B54-9BED3B34F518"), 300);
            ColumnXStore = new ColumnXStore(this);
            SymbolXStore = new StoreOf<SymbolX>(this, Trait.SymbolXStore, new System.Guid("4ED54C41-4EDD-41D6-8451-2FEF0967C12F"), 100);
            ComputeXStore = new StoreOf<ComputeX>(this, Trait.ComputeXStore, new System.Guid("A3F850B4-B498-4339-94B8-4F0E355BAD92"), 300);
            RelationXStore = new RelationXStore(this);

            PrimeStores = new Store[]
            {
                EnumXStore,
                ViewXStore,
                TableXStore,
                GraphXStore,
                QueryXStore,
                ColumnXStore,
                SymbolXStore,
                ComputeXStore,
                RelationXStore,
                RelationStore,
                PropertyStore,
            };
        }
        #endregion

        #region ReleaseStores  ================================================
        private void ReleaseStores()
        {
            Dummy = null;

            QueryXNode = null;
            ChangeRoot = null;
            ChangeSet = null;

            PropertyZStore = null;
            RelationZStore = null;
            EnumZStore = null;
            ErrorStore = null;

            PropertyStore = null;
            RelationStore = null;
            EnumXStore = null;
            ViewXStore = null;
            TableXStore = null;
            GraphXStore = null;
            QueryXStore = null;
            ColumnXStore = null;
            SymbolXStore = null;
            ComputeXStore = null;
            RelationXStore = null;

            PrimeStores = null;
        }
        #endregion
    }
}
