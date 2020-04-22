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

        internal InternalDomainOf<Property> PropertyStore { get; private set; }
        internal InternalDomainOf<Relation> RelationStore { get; private set; }

        internal DummyItem DummyItemRef { get; private set; }
        internal QueryX DummyQueryXRef { get; private set; }

        internal ChangeSet ChangeSet { get; private set; }
        internal ChangeRoot ChangeRoot { get; private set; }
        internal int ChangeSequence { get; private set; }

        internal Store[] PrimeStores { get; private set; }

        internal EnumXDomain EnumXStore { get; private set; }
        internal ViewXDomain ViewXStore { get; private set; }
        internal TableXDomain TableXStore { get; private set; }
        internal GraphXDomain GraphXStore { get; private set; }
        internal GraphParams GraphParams { get; private set; }
        internal QueryXDomain QueryXStore { get; private set; }
        internal ColumnXDomain ColumnXStore { get; private set; }
        internal SymbolXDomain SymbolXStore { get; private set; }
        internal ComputeXDomain ComputeXStore { get; private set; }
        internal RelationXDomain RelationXStore { get; private set; }

        #region InitializeStores  =============================================
        private void InitializeStores()
        {
            DummyItemRef = new DummyItem(this);
            DummyQueryXRef = new DummyQueryX(this);

            ChangeRoot = new ChangeRoot(this);
            ChangeSequence = 1;
            ChangeSet = new ChangeSet(ChangeRoot, ChangeSequence);

            ErrorStore = new StoreOf<Error>(this, IdKey.ErrorStore, 20);
            EnumZStore = new StoreOf<EnumZ>(this, IdKey.EnumZStore, 20);
            PropertyZStore = new StoreOf<Property>(this, IdKey.PropertyZStore, 10);
            RelationZStore = new StoreOf<Relation>(this, IdKey.RelationZStore, 10);

            PropertyStore = new InternalDomainOf<Property>(this, IdKey.PropertyStore, 100);
            RelationStore = new RelationDomain(this);

            EnumXStore = new EnumXDomain(this);
            ViewXStore = new ViewXDomain(this);
            TableXStore = new TableXDomain(this);
            GraphXStore = new GraphXDomain(this);
            GraphParams = new GraphParams(this, GraphXStore);

            QueryXStore = new QueryXDomain(this);
            ColumnXStore = new ColumnXDomain(this);
            SymbolXStore = new SymbolXDomain(this);
            ComputeXStore = new ComputeXDomain(this);
            RelationXStore = new RelationXDomain(this);

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
