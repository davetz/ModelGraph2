using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public partial class Chef
    {

        internal StoreOf<Error> ErrorStore { get; private set; }
        internal StoreOf<EnumZ> EnumZStore { get; private set; }
        internal StoreOf<Property> PropertyZStore { get; private set; }
        internal StoreOf<Relation> RelationZStore { get; private set; }

        internal InternalStoreOf<Property> PropertyStore { get; private set; }
        internal InternalStoreOf<Relation> RelationStore { get; private set; }

        internal DummyItem DummyItemRef { get; private set; }
        internal QueryX DummyQueryXRef { get; private set; }

        internal ChangeSet ChangeSet { get; private set; }
        internal ChangeRoot ChangeRoot { get; private set; }
        internal int ChangeSequence { get; private set; }

        internal Store[] PrimeStores { get; private set; }

        internal EnumXStore EnumXStore { get; private set; }
        internal ViewXStore ViewXStore { get; private set; }
        internal TableXStore TableXStore { get; private set; }
        internal GraphXStore GraphXStore { get; private set; }
        internal GraphParams GraphParams { get; private set; }
        internal QueryXStore QueryXStore { get; private set; }
        internal ColumnXStore ColumnXStore { get; private set; }
        internal SymbolXStore SymbolXStore { get; private set; }
        internal ComputeXStore ComputeXStore { get; private set; }
        internal RelationXStore RelationXStore { get; private set; }

        #region InitializeStores  =============================================
        private void InitializeStores()
        {
            DummyItemRef = new DummyItem(this);
            DummyQueryXRef = new DummyQueryX(this);

            ChangeRoot = new ChangeRoot(this);
            ChangeSequence = 1;
            ChangeSet = new ChangeSet(ChangeRoot, ChangeSequence);

            ErrorStore = new StoreOf<Error>(this, Trait.ErrorStore, 20);
            EnumZStore = new StoreOf<EnumZ>(this, Trait.EnumZStore, 20);
            PropertyZStore = new StoreOf<Property>(this, Trait.PropertyZStore, 10);
            RelationZStore = new StoreOf<Relation>(this, Trait.RelationZStore, 10);

            PropertyStore = new InternalStoreOf<Property>(this, Trait.PropertyStore, 100);
            RelationStore = new RelationStore(this);

            EnumXStore = new EnumXStore(this);
            ViewXStore = new ViewXStore(this);
            TableXStore = new TableXStore(this);
            GraphXStore = new GraphXStore(this);
            GraphParams = new GraphParams(this, GraphXStore);

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
