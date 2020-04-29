using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public partial class Chef
    {
        private int ChangeSequence = 1;
        private readonly Dictionary<Type, Item> Type_InstanceOf = new Dictionary<Type, Item>(200);
        private readonly Dictionary<ushort, Item> IdKey_ReferenceItem = new Dictionary<ushort, Item>(200);

        internal void RegisterPrivateItem(Item item) => Type_InstanceOf[item.GetType()] = item;

        internal void RegisterReferenceItem(Item item) { IdKey_ReferenceItem[item.ItemKey] = item; Type_InstanceOf[item.GetType()] = item; }

        internal T GetItem<T>() where T : Item
        {
            if (Type_InstanceOf.TryGetValue(typeof(T), out Item itm) && itm is T val)
                return val;
            throw new InvalidOperationException($"Chef GetItem<T>() : could not find type {typeof(T)}");
        }

        #region PrimeStores  ==================================================
        internal Store[] PrimeStores => new Store[]
        {
            GetItem<EnumXDomain>(),
            GetItem<ViewXDomain>(),
            GetItem<TableXDomain>(),
            GetItem<GraphXDomain>(),
            GetItem<QueryXDomain>(),
            GetItem<ColumnXDomain>(),
            GetItem<SymbolXDomain>(),
            GetItem<ComputeXDomain>(),
            GetItem<RelationXDomain>(),
            GetItem<RelationDomain>(),
            GetItem<PropertyDomain>(),
        };
        #endregion

        #region InitializeDomains  ============================================
        private void InitializeDomains()
        {
            RegisterReferenceItem(new DummyItem(this));
            RegisterReferenceItem(new DummyQueryX(this));

            RegisterPrivateItem(new ChangeRoot(this));

            RegisterPrivateItem(new ChangeSet(GetItem<ChangeRoot>(), ChangeSequence));

            RegisterPrivateItem(new ErrorStore(this));
            RegisterPrivateItem(new EnumZStore(this));
            InitializeEnums();

            RegisterPrivateItem(new PropertyZStore(this));
            RegisterPrivateItem(new RelationZStore(this));

            RegisterReferenceItem(new PropertyDomain(this));
            RegisterReferenceItem(new RelationDomain(this));

            RegisterReferenceItem(new EnumXDomain(this));
            RegisterReferenceItem(new ViewXDomain(this));
            RegisterReferenceItem(new TableXDomain(this));
            RegisterReferenceItem(new GraphXDomain(this));
            RegisterReferenceItem(new GraphParams(this, GetItem<GraphXDomain>()));

            RegisterReferenceItem(new QueryXDomain(this));
            RegisterReferenceItem(new ColumnXDomain(this));
            RegisterReferenceItem(new SymbolXDomain(this));
            RegisterReferenceItem(new ComputeXDomain(this));
            RegisterReferenceItem(new RelationXDomain(this));
        }
        #endregion

        #region Override Chef.Discard()  ======================================
        /// <summary>Remove pointers to all objects that were created by this dataChef</summary>
        internal override void Discard()
        {
            Type_InstanceOf.Clear();
            IdKey_ReferenceItem.Clear();
            base.Discard();
        }
        #endregion
    }
}
