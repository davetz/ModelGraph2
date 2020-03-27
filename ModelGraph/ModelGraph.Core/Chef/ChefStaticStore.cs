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
        internal TableXStore TableXStore { get; private set; }
        internal GraphXStore GraphXStore { get; private set; }
        internal QueryXStore QueryXStore { get; private set; }
        internal ColumnXStore ColumnXStore { get; private set; }
        internal SymbolXStore SymbolXStore { get; private set; }
        internal ComputeXStore ComputeXStore { get; private set; }
        internal RelationXStore RelationXStore { get; private set; }

        #region InitializeStores  =============================================
        private void InitializeStores()
        {
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
            RelationStore = new RelationStore(this);

            EnumXStore = new EnumXStore(this);
            ViewXStore = new ViewXStore(this);
            TableXStore = new TableXStore(this);
            GraphXStore = new GraphXStore(this);
            QueryXStore = new QueryXStore(this);
            ColumnXStore = new ColumnXStore(this);
            SymbolXStore = new SymbolXStore(this);
            ComputeXStore = new ComputeXStore(this);
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
    }
}
