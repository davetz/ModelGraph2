using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public partial class Chef
    {
        private readonly Dictionary<Type, Property[]> _itemType_Properties = new Dictionary<Type, Property[]>(100);

        internal PropertyOf<Chef, bool> ShowItemIndexProperty;

        #region CreateProperties  =============================================
        private void CreateProperties()
        {
            {
                var p = ShowItemIndexProperty = new PropertyOf<Chef, bool>(PropertyStore, Trait.IncludeItemIdentityIndex_P);
                p.GetValFunc = (item) => p.Cast(item).ShowItemIndex;
                p.SetValFunc = (item, value) => p.Cast(item).ShowItemIndex = value;
                p.Value = new BoolValue(p);
            }
        }
        #endregion

        #region RegisterStaticProperties  =============================================
        internal void RegisterStaticProperties(Type type, List<Property> props)
        {
            _itemType_Properties.Add(type, props.ToArray());
        }
        #endregion

        #region LookUpProperty  ===============================================
        internal bool TryLookUpProperty(Store store, string name, out Property prop)
        {
            prop = null;

            if (string.IsNullOrWhiteSpace(name)) return false;

            if (TableX_ColumnX.TryGetChildren(store, out IList<ColumnX> ls1))
            {
                foreach (var col in ls1)
                {
                    if (string.IsNullOrWhiteSpace(col.Name)) continue;
                    if (string.Compare(col.Name, name, true) == 0) { prop = col; return true; }
                }
            }
            if (Store_ComputeX.TryGetChildren(store, out IList<ComputeX> ls2))
            {
                foreach (var cd in ls2)
                {
                    var n = cd.Name;
                    if (string.IsNullOrWhiteSpace(n)) continue;
                    if (string.Compare(n, name, true) == 0) { prop = cd; return true; }
                }
            }
            if (_itemType_Properties.TryGetValue(store.GetChildType(), out Property[] arr))
            {
                foreach (var pr in arr)
                {
                    if (string.Compare(name, _localize(pr.NameKey), true) == 0) { prop = pr; return true; }
                }
            }
            return false;
        }
        internal bool TryLookUpProperty(Type type, string name, out Property prop)
        {
            prop = null;

            if (string.IsNullOrWhiteSpace(name)) return false;

            if (_itemType_Properties.TryGetValue(type, out Property[] arr))
            {
                foreach (var pr in arr)
                {
                    if (string.Compare(name, _localize(pr.NameKey), true) == 0) { prop = pr; return true; }
                }
            }
            return false;
        }
        #endregion

        #region Helpers  ======================================================

        internal string GetQueryXRelationName(Item item)
        {
            return Relation_QueryX.TryGetParent(item, out Relation rel) ? GetRelationName(rel) : string.Empty;
        }
        #endregion
    }
}
