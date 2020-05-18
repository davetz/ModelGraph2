using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public partial class Chef
    {
        private int ChangeSequence = 1;
        private readonly Dictionary<Type, Item> Type_InstanceOf = new Dictionary<Type, Item>(200);  // used to get a specific type instance
        private readonly Dictionary<ushort, Item> IdKey_ReferenceItem = new Dictionary<ushort, Item>(200); // used to get specific type from its IdKey
        private readonly Dictionary<Type, Property[]> Type_Properties = new Dictionary<Type, Property[]>(100); // used for property name lookup

        private readonly List<(Guid, ISerializer)> ItemSerializers = new List<(Guid, ISerializer)>(20);
        private readonly List<(Guid, ISerializer)> LinkSerializers = new List<(Guid, ISerializer)>(10);

        public void RegisterItemSerializer((Guid, ISerializer) serializer)
        {
            if (ItemSerializers.Count == 0)
                ItemSerializers.Add((_serilizerGuid, this)); //the internal reference serializer should be first

            ItemSerializers.Add(serializer); //item serializers added according to registration order
        }
        public void RegisterLinkSerializer((Guid, ISerializer) serializer) => LinkSerializers.Add(serializer); //link serializers will be called last

        internal void RegisterStaticProperties(Type type, Property[] props) => Type_Properties.Add(type, props);

        internal void RegisterPrivateItem(Item item) => Type_InstanceOf[item.GetType()] = item;

        internal void RegisterReferenceItem(Item item) { IdKey_ReferenceItem[item.ItemKey] = item; Type_InstanceOf[item.GetType()] = item; }
        
        internal T Get<T>() where T : Item
        {
            if (Type_InstanceOf.TryGetValue(typeof(T), out Item itm) && itm is T val)
                return val;
            throw new InvalidOperationException($"Chef GetItem<T>() : could not find type {typeof(T)}");
        }


        #region LookUpProperty  ===============================================
        internal bool TryLookUpProperty(Store store, string name, out Property prop)
        {
            prop = null;

            if (string.IsNullOrWhiteSpace(name)) return false;

            if (Get<Relation_Store_ColumnX>().TryGetChildren(store, out IList<ColumnX> ls1))
            {
                foreach (var col in ls1)
                {
                    if (string.IsNullOrWhiteSpace(col.Name)) continue;
                    if (string.Compare(col.Name, name, true) == 0) { prop = col; return true; }
                }
            }
            if (Get<Relation_Store_ComputeX>().TryGetChildren(store, out IList<ComputeX> ls2))
            {
                foreach (var cd in ls2)
                {
                    var n = cd.Name;
                    if (string.IsNullOrWhiteSpace(n)) continue;
                    if (string.Compare(n, name, true) == 0) { prop = cd; return true; }
                }
            }
            if (Type_Properties.TryGetValue(store.GetChildType(), out Property[] arr))
            {
                foreach (var pr in arr)
                {
                    if (string.Compare(name, _localize(pr.NameKey), true) == 0) { prop = pr; return true; }
                }
            }
            return false;
        }
        #endregion

        #region PrimeStores  ==================================================
        internal Store[] PrimeStores => new Store[]
        {
            Get<EnumXRoot>(),
            Get<ViewXRoot>(),
            Get<TableXRoot>(),
            Get<GraphXRoot>(),
            Get<QueryXRoot>(),
            Get<ColumnXRoot>(),
            Get<SymbolXRoot>(),
            Get<ComputeXRoot>(),
            Get<RelationXRoot>(),
            Get<RelationRoot>(),
            Get<PropertyRoot>(),
        };
        #endregion

        #region InitializeDomains  ============================================
        private void InitializeDomains()
        {
            RegisterReferenceItem(new DummyItem(this));
            RegisterReferenceItem(new DummyQueryX(this));

            RegisterReferenceItem(new RelationRoot(this));
            RegisterReferenceItem(new PropertyRoot(this));
            RegisterPrivateItem(new ChangeRoot(this));
            RegisterPrivateItem(new ErrorRoot(this));
            RegisterPrivateItem(new EnumRoot(this));

            RegisterReferenceItem(new EnumXRoot(this));
            RegisterReferenceItem(new ViewXRoot(this));
            RegisterReferenceItem(new TableXRoot(this));
            RegisterReferenceItem(new GraphXRoot(this));

            RegisterReferenceItem(new QueryXRoot(this));
            RegisterReferenceItem(new ColumnXRoot(this));
            RegisterReferenceItem(new SymbolXRoot(this));
            RegisterReferenceItem(new ComputeXRoot(this));
            RegisterReferenceItem(new RelationXRoot(this));
        }
        #endregion

        #region Override Chef.Discard()  ======================================
        /// <summary>Remove references that were created by this dataChef</summary>
        internal override void Discard()
        {
            Type_Properties.Clear();
            Type_InstanceOf.Clear();
            IdKey_ReferenceItem.Clear();
            ItemSerializers.Clear();
            LinkSerializers.Clear();

            base.Discard();
        }
        #endregion
    }
}
