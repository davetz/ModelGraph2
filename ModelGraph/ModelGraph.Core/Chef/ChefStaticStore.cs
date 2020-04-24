using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public partial class Chef
    {
        internal DummyItem DummyItemRef { get; private set; }
        internal QueryX DummyQueryXRef { get; private set; }

        internal ChangeSet ChangeSet { get; private set; }
        internal ChangeRoot ChangeRoot { get; private set; }
        internal int ChangeSequence { get; private set; }

        internal ErrorStore ErrorStore { get; private set; }
        internal EnumZStore EnumZStore { get; private set; }
        internal PropertyZStore PropertyZStore { get; private set; }
        internal RelationZStore RelationZStore { get; private set; }

        internal PropertyDomain PropertyDomain { get; private set; }
        internal RelationDomain RelationDomain { get; private set; }

        internal Store[] PrimeStores { get; private set; }

        internal EnumXDomain EnumXDomain { get; private set; }
        internal ViewXDomain ViewXDomain { get; private set; }
        internal TableXDomain TableXDomain { get; private set; }
        internal GraphXDomain GraphXDomain { get; private set; }
        internal GraphParams GraphParams { get; private set; }
        internal QueryXDomain QueryXDomain { get; private set; }
        internal ColumnXDomain ColumnXDomain { get; private set; }
        internal SymbolXDomain SymbolXDomain { get; private set; }
        internal ComputeXDomain ComputeXDomain { get; private set; }
        internal RelationXDomain RelationXDomain { get; private set; }

        #region InitializeStores  =============================================
        private void InitializeStores()
        {
            DummyItemRef = new DummyItem(this);
            DummyQueryXRef = new DummyQueryX(this);

            ChangeRoot = new ChangeRoot(this);
            ChangeSequence = 1;
            ChangeSet = new ChangeSet(ChangeRoot, ChangeSequence);

            ErrorStore = new ErrorStore(this);
            EnumZStore = new EnumZStore(this);
            InitializeEnums();

            PropertyZStore = new PropertyZStore(this);
            RelationZStore = new RelationZStore(this);

            PropertyDomain = new PropertyDomain(this);
            RelationDomain = new RelationDomain(this);

            EnumXDomain = new EnumXDomain(this);
            ViewXDomain = new ViewXDomain(this);
            TableXDomain = new TableXDomain(this);
            GraphXDomain = new GraphXDomain(this);
            GraphParams = new GraphParams(this, GraphXDomain);

            QueryXDomain = new QueryXDomain(this);
            ColumnXDomain = new ColumnXDomain(this);
            SymbolXDomain = new SymbolXDomain(this);
            ComputeXDomain = new ComputeXDomain(this);
            RelationXDomain = new RelationXDomain(this);

            PrimeStores = new Store[]
            {
                EnumXDomain,
                ViewXDomain,
                TableXDomain,
                GraphXDomain,
                QueryXDomain,
                ColumnXDomain,
                SymbolXDomain,
                ComputeXDomain,
                RelationXDomain,
                RelationDomain,
                PropertyDomain,
            };
        }
        #endregion
    }
}
