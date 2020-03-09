using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{/*

 */
    public partial class Chef
    {
        internal static IList<Item> EmptyItemList = new List<Item>(0).AsReadOnly();
        internal Dummy Dummy { get; private set; }

        internal ImportBinaryReader ImportBinaryReader { get; private set; }
        internal ExportBinaryWriter ExportBinaryWriter { get; private set; }
        internal QueryX QueryXNode { get; private set; }

        internal ChangeSet ChangeSet { get; private set; }

        internal ChangeRoot ChangeRoot { get; private set; }

        internal int ChangeSequence { get; private set; }

        internal Store[] PrimeStores { get; private set; }
        internal StoreOfOld<EnumX> EnumXStore { get; private set; }

        internal StoreOfOld<ViewX> ViewXStore { get; private set; }

        internal StoreOfOld<TableX> TableXStore { get; private set; }

        internal StoreOfOld<GraphX> GraphXStore { get; private set; }

        internal StoreOfOld<QueryX> QueryXStore { get; private set; }

        internal StoreOfOld<ColumnX> ColumnXStore { get; private set; }

        internal StoreOfOld<SymbolX> SymbolStore { get; private set; }

        internal StoreOfOld<ComputeX> ComputeXStore { get; private set; }

        internal StoreOfOld<RelationX> RelationXStore { get; private set; }

        internal StoreOfOld<Error> ErrorStore { get; private set; }
        internal StoreOfOld<EnumZ> EnumZStore { get; private set; }

        internal StoreOfOld<Property> PropertyStore { get; private set; }

        internal StoreOfOld<Relation> RelationStore { get; private set; }

        internal StoreOfOld<Property> PropertyZStore { get; private set; }

        internal StoreOfOld<Relation> RelationZStore { get; private set; }

        #region InitializeStores  =============================================
        private void InitializeStores()
        {
            Dummy = new Dummy(this);
            ImportBinaryReader = new ImportBinaryReader(this);
            ExportBinaryWriter = new ExportBinaryWriter(this);

            QueryXNode = new QueryX(this);
            ChangeRoot = new ChangeRoot(this);
            ChangeSequence = 1;
            ChangeSet = new ChangeSet(ChangeRoot, ChangeSequence);

            PropertyZStore = new StoreOfOld<Property>(this, Trait.PropertyStore, Guid.Empty, 30);
            RelationZStore = new StoreOfOld<Relation>(this, Trait.RelationZStore, Guid.Empty, 10);
            EnumZStore = new StoreOfOld<EnumZ>(this, Trait.EnumZStore, Guid.Empty, 10);
            ErrorStore = new StoreOfOld<Error>(this, Trait.ErrorStore, Guid.Empty, 10);

            PropertyStore = new StoreOfOld<Property>(this, Trait.PropertyStore, new System.Guid("BA10F400-9A33-4F65-80A1-C2259D17A938"), 100);
            RelationStore = new StoreOfOld<Relation>(this, Trait.RelationStore, new System.Guid("42743CEF-2172-4C55-A575-9A26357E4FB5"), 30);
            EnumXStore = new StoreOfOld<EnumX>(this, Trait.EnumXStore, new System.Guid("EC7B6089-AD64-4100-8F65-BA8130969EB0"), 10);
            ViewXStore = new StoreOfOld<ViewX>(this, Trait.ViewXStore, new System.Guid("C11EAF6E-20A2-4F2E-AF19-0BC49DF561AB"), 10);
            TableXStore = new StoreOfOld<TableX>(this, Trait.TableXStore, new System.Guid("0E00F963-18F6-4C7C-A6E9-71C4CCE001DC"), 30);
            GraphXStore = new StoreOfOld<GraphX>(this, Trait.GraphXStore, new System.Guid("72C2BEC8-B8C8-44A1-ADF0-3832416820F3"), 30);
            QueryXStore = new StoreOfOld<QueryX>(this, Trait.QueryXStore, new System.Guid("085A1887-03FE-4DA1-9B54-9BED3B34F518"), 300);
            ColumnXStore = new StoreOfOld<ColumnX>(this, Trait.ColumnXStore, new System.Guid("44F4B1B2-927C-40DF-A8E6-60A1E4DA58A6"), 300);
            SymbolStore = new StoreOfOld<SymbolX>(this, Trait.SymbolXStore, new System.Guid("4ED54C41-4EDD-41D6-8451-2FEF0967C12F"), 100);
            ComputeXStore = new StoreOfOld<ComputeX>(this, Trait.ComputeXStore, new System.Guid("A3F850B4-B498-4339-94B8-4F0E355BAD92"), 300);
            RelationXStore = new StoreOfOld<RelationX>(this, Trait.RelationXStore, new System.Guid("BD104B70-CB79-42C3-858D-588B6B868269"), 300);

            PrimeStores = new Store[]
            {
                EnumXStore,
                ViewXStore,
                TableXStore,
                GraphXStore,
                QueryXStore,
                ColumnXStore,
                SymbolStore,
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
            ImportBinaryReader = null;
            ExportBinaryWriter = null;

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
            SymbolStore = null;
            ComputeXStore = null;
            RelationXStore = null;

            PrimeStores = null;
        }
        #endregion
    }
}
