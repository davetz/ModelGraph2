﻿using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public partial class Chef
    {
        private Func<string, string> _localize = (s) => s; //dummy default localizer
        public void SetLocalizer(Func<string, string> localizer) => _localize = localizer;

        private Dictionary<IdKey, Func<Item, IdentityStyle, string>> _itemIdentity;

        #region GetIdentity  ==================================================
        public string GetIdentity(Item item, IdentityStyle style)
        {
            if (item != null && _itemIdentity.TryGetValue(item.IdKey, out Func<Item, IdentityStyle, string> id))
                return id(item, style);
            else if (item is Relation)
                return Relation_Identity(item, style);
            else if (item is Property)
                return Property_Identity(item, style);
            else
                return Undefined_Identity(item, style);
        }
        #endregion

        #region InitializeItemIdentity  =======================================
        private void InitializeItemIdentity()
        {
            _itemIdentity = new Dictionary<IdKey, Func<Item, IdentityStyle, string>>(73)
            {
                { IdKey.RowX, RowX_Identity },
                { IdKey.ViewX, ViewX_Identity },
                { IdKey.PairX, PairX_Identity },
                { IdKey.EnumX, EnumX_Identity },
                { IdKey.TableX, TableX_Identity },
                { IdKey.GraphX, GraphX_Identity },
                { IdKey.QueryX, QueryX_Identity },
                { IdKey.SymbolX, SymbolX_Identity },
                { IdKey.ColumnX, ColumnX_Identity },
                { IdKey.ComputeX, ComputeX_Identity },
                //_itemIdentity.Add(IdKey.CommandX, CommandX_Identity);
                { IdKey.RelationX, RelationX_Identity }
            };
        }
        #endregion

        #region Undefined  ====================================================
        private string Undefined_Identity(Item item, IdentityStyle style)
        {
            if (item is null) return BlankName;
            //if (item.TraitKey == 0x1AD) item.SetTrait(IdKey.QueryXNodeSymbol);
            return $"Internal {item.IdKey.ToString()}";
        }
        private string Index_Identity(Item item)
        {
            if (!(item.Owner is Store own)) return InvalidItem;

            var inx = own.IndexOf(item);
            if (inx < 0) return InvalidItem;

            return $"#{inx}";
        }
        #endregion

        #region RowX_Identity  ================================================
        private string RowX_Identity(Item item, IdentityStyle style = IdentityStyle.Single)
        {
            if (item is RowX rx)
            {
                var name1 = TableX_NameProperty.TryGetChild(rx.TableX, out Property pr) ? pr.Value.GetString(rx) : null;
                var noName1 = string.IsNullOrWhiteSpace(name1);
                if (noName1) name1 = $"#{rx.Index}";

                var name2 = rx.TableX.Name;
                var noName2 = string.IsNullOrWhiteSpace(name2);
                if (noName2) name2 = $"{_localize(rx.TableX.KindKey)}[{rx.TableX.Index}]";

                switch (style)
                {
                    case IdentityStyle.Single:
                        return name1;

                    case IdentityStyle.Double:
                    case IdentityStyle.StoreItem:
                        return $"{name2} : {name1}";


                    case IdentityStyle.ChangeLog:
                        return $"{name2} : {name1}";


                    case IdentityStyle.Kind:
                        return _localize(rx.KindKey);

                    case IdentityStyle.Summary:
                        return TableX_SummaryProperty.TryGetChild(rx.TableX, out Property ns) ? ns.Value.GetString(rx) : name1;

                    case IdentityStyle.Description:
                        return null;
                }
            }
            return $"{BlankName} {item.IdKey}";
        }
        #endregion

        #region PairX_Identity  ===============================================
        private string PairX_Identity(Item item, IdentityStyle style)
        {
            if (item is PairX px)
            {
                var name1 = px.DisplayValue;
                var noName1 = string.IsNullOrWhiteSpace(name1);
                if (noName1) name1 = $"{_localize(px.KindKey)} {BlankName}";

                var name2 = px.EnumX.Name;
                var noName2 = string.IsNullOrWhiteSpace(name2);
                if (noName2) name2 = $"{_localize(px.EnumX.KindKey)}[{px.EnumX.Index}]";

                switch (style)
                {
                    case IdentityStyle.Single:
                        return name1;

                    case IdentityStyle.Double:
                    case IdentityStyle.StoreItem:
                        return $"{name2} : {name1}";

                    case IdentityStyle.ChangeLog:
                        return $"{name2} : {name1}";

                    case IdentityStyle.Kind:
                        return _localize(px.KindKey);;

                    case IdentityStyle.Summary:
                        return px.ActualValue;

                    case IdentityStyle.Description:
                        return null;
                }
            }
            return $"{BlankName} {item.IdKey.ToString()}";
        }
        #endregion

        #region ViewX_Identity  ===============================================
        private string ViewX_Identity(Item item, IdentityStyle style)
        {
            if (item is ViewX itm)
            {
                var name = itm.Name;
                if (string.IsNullOrWhiteSpace(name)) name = $"{_localize(itm.KindKey)} {Index_Identity(itm)}";

                switch (style)
                {
                    case IdentityStyle.Single:
                    case IdentityStyle.Double:
                    case IdentityStyle.StoreItem:
                        return name;

                    case IdentityStyle.ChangeLog:
                        return $"{_localize(itm.KindKey)} {Index_Identity(itm)}: {name}";

                    case IdentityStyle.Kind:
                        return _localize(itm.KindKey);;

                    case IdentityStyle.Summary:
                        return itm.Summary;

                    case IdentityStyle.Description:
                        return itm.Description;
                }
            }
            return $"{BlankName} {item.IdKey.ToString()}";
        }
        #endregion

        #region EnumX_Identity  ===============================================
        private string EnumX_Identity(Item item, IdentityStyle style)
        {
            if (item is EnumX ex)
            {
                var name = ex.Name;
                var noName = string.IsNullOrWhiteSpace(name);
                if (noName) name = $"#{ex.Index}";

                switch (style)
                {
                    case IdentityStyle.Single:
                    case IdentityStyle.Double:
                    case IdentityStyle.StoreItem:
                        return name;

                    case IdentityStyle.ChangeLog:
                        return $"{_localize(ex.KindKey)} {name}";

                    case IdentityStyle.Kind:
                        return _localize(ex.KindKey);;

                    case IdentityStyle.Summary:
                        return ex.Summary;

                    case IdentityStyle.Description:
                        return ex.Description;
                }
            }
            return $"{BlankName} {item.IdKey.ToString()}";
        }
        #endregion

        #region TableX_Identity  ==============================================
        private string TableX_Identity(Item item, IdentityStyle style)
        {
            if (item is TableX tx)
            {
                var name = tx.Name;
                var noName = string.IsNullOrWhiteSpace(name);
                if (noName) name = $"#{tx.Index}";

                switch (style)
                {
                    case IdentityStyle.Single:
                    case IdentityStyle.StoreItem:
                        return name;

                    case IdentityStyle.Double:
                        return $"{_localize(tx.KindKey)} : {name}";

                    case IdentityStyle.ChangeLog:
                        return $"{tx.Index} {_localize(tx.KindKey)}: {name}";

                    case IdentityStyle.Kind:
                        return _localize(tx.KindKey);

                    case IdentityStyle.Summary:
                        return tx.Summary;

                    case IdentityStyle.Description:
                        return tx.Description;
                }
            }
            return $"{BlankName} {item.IdKey.ToString()}";
        }
        #endregion

        #region GraphX_Identity  ==============================================
        private string GraphX_Identity(Item item, IdentityStyle style)
        {
            if (item is GraphX itm)
            {
                var name = itm.Name;
                if (string.IsNullOrWhiteSpace(name)) name = $"{_localize(itm.KindKey)} {Index_Identity(itm)}";

                switch (style)
                {
                    case IdentityStyle.Single:
                    case IdentityStyle.Double:
                    case IdentityStyle.StoreItem:
                        return name;

                    case IdentityStyle.ChangeLog:
                        return $"{_localize(itm.KindKey)} {Index_Identity(itm)}: {name}";

                    case IdentityStyle.Kind:
                        return _localize(itm.KindKey);

                    case IdentityStyle.Summary:
                        return itm.Summary;

                    case IdentityStyle.Description:
                        return itm.Description;
                }
            }
            return $"{BlankName} {item.IdKey.ToString()}";
        }
        #endregion

        #region QueryX_Identity  ==============================================
        private string QueryX_Identity(Item item, IdentityStyle style)
        {
            if (item is QueryX qx)
            {
                IdKey idKe = IdKey.QueryIsCorrupt;
                switch (qx.QueryKind)
                {
                    case QueryType.View:
                        if (qx.IsHead)
                            idKe = IdKey.QueryViewHead;
                        else if (qx.IsRoot)
                            idKe = IdKey.QueryViewRoot;
                        else
                            idKe = IdKey.QueryViewLink;
                        break;
                    case QueryType.Path:
                        if (qx.IsHead)
                            idKe = IdKey.QueryPathHead;
                        else
                            idKe = IdKey.QueryPathLink;
                        break;
                    case QueryType.Group:
                        if (qx.IsHead)
                            idKe = IdKey.QueryGroupHead;
                        else
                            idKe = IdKey.QueryGroupLink;
                        break;
                    case QueryType.Egress:
                        if (qx.IsHead)
                            idKe = IdKey.QuerySegueHead;
                        else
                            idKe = IdKey.QuerySegueLink;
                        break;
                    case QueryType.Graph:
                        if (qx.IsRoot)
                            idKe = IdKey.QueryGraphRoot;
                        else
                            idKe = IdKey.QueryGraphLink;
                        break;
                    case QueryType.Value:
                        if (qx.IsHead)
                            idKe = IdKey.QueryValueHead;
                        else if (qx.IsRoot)
                            idKe = IdKey.QueryValueRoot;
                        else
                            idKe = IdKey.QueryValueLink;
                        break;
                    case QueryType.Symbol:
                        idKe = IdKey.QueryNodeSymbol;
                        break;
                }
                var kind = _localize(GetKindKey(idKe));
                var name = string.Empty;
                if (qx.IsRoot)
                {
                    if (Store_QueryX.TryGetParent(qx, out Store st))
                        name = GetIdentity(st, IdentityStyle.Double);
                    else
                        name = InvalidItem;
                }
                else
                {
                    if (Relation_QueryX.TryGetParent(qx, out Relation re))
                    {
                        GetHeadTail(qx, out Store head, out Store tail);
                        name = $"{GetIdentity(head, IdentityStyle.Single)} --> {GetIdentity(tail, IdentityStyle.Single)}";
                    }
                    else
                        name = InvalidItem;
                }

                if (qx.HasSelect || qx.HasWhere || qx.IsRoot || qx.IsHead || qx.IsTail)
                {
                    name = $"{name}      [";
                    if (qx.HasWhere) name = $"{name}{GetName(IdKey.QueryWhere)}( {qx.WhereString} )";
                    if (qx.HasSelect) name = $"{name} {GetName(IdKey.QuerySelect)}( {qx.SelectString} )";

                    if (qx.IsRoot || qx.IsHead || qx.IsTail)
                    {
                        name = $"{name} ";
                        if (qx.IsRoot) name = $"{name}R";
                        if (qx.IsHead) name = $"{name}H";
                        if (qx.IsTail) name = $"{name}T";
                    }
                    name = $"{name}]";
                }


                switch (style)
                {
                    case IdentityStyle.Single:
                    case IdentityStyle.Double:
                    case IdentityStyle.StoreItem:
                        return name;

                    case IdentityStyle.ChangeLog:
                        return $"{kind} {Index_Identity(qx)}: {name}";

                    case IdentityStyle.Kind:
                        return kind;

                    case IdentityStyle.Summary:
                        return _localize(GetSummaryKey(idKe));

                    case IdentityStyle.Description:
                        return _localize(GetDescriptionKey(idKe));
                }
            }
            return $"{BlankName} {item.IdKey.ToString()}";
        }
        #endregion

        #region SymbolX_Identity  =============================================
        private string SymbolX_Identity(Item item, IdentityStyle style)
        {
            if (item is SymbolX itm)
            {
                var name = itm.Name;
                if (string.IsNullOrWhiteSpace(name)) name = $"{_localize(itm.KindKey)} {Index_Identity(itm)}";

                ;
                var parent = GraphX_SymbolX.TryGetParent(itm, out GraphX pa) ? pa.Name : BlankName;
                if (string.IsNullOrWhiteSpace(parent)) parent = $"{_localize(pa.KindKey)} {Index_Identity(pa)}";

                switch (style)
                {
                    case IdentityStyle.Single:
                        return name;

                    case IdentityStyle.Double:
                    case IdentityStyle.StoreItem:
                        return $"{parent} : {name}";

                    case IdentityStyle.ChangeLog:
                        return $"{_localize(itm.KindKey)} {Index_Identity(itm)}: {parent} : {name}";

                    case IdentityStyle.Kind:
                        return _localize(itm.KindKey);;

                    case IdentityStyle.Summary:
                        return itm.Summary;

                    case IdentityStyle.Description:
                        return null;
                }
            }
            return $"{BlankName} {item.IdKey.ToString()}";
        }
        #endregion

        #region ColumnX_Identity  =============================================
        private string ColumnX_Identity(Item item, IdentityStyle style)
        {
            if (item is ColumnX cx)
            {
                var name1 = cx.Name;
                var noName1 = string.IsNullOrWhiteSpace(name1);
                if (noName1) name1 = $"#{cx.Index}";

                var name2 = TableX_ColumnX.TryGetParent(cx, out TableX tx) ? tx.Name : null;
                var noName2 = string.IsNullOrWhiteSpace(name2);
                if (noName2) name2 = $"{_localize(GetKindKey(IdKey.TableX))} {BlankName}";

                switch (style)
                {
                    case IdentityStyle.Single:
                        return name1;

                    case IdentityStyle.Double:
                    case IdentityStyle.StoreItem:
                        return $"{name2} {name1}";

                    case IdentityStyle.ChangeLog:
                        return $"{cx.Index} {_localize(cx.KindKey)} {name2} {name1}";

                    case IdentityStyle.Kind:
                        return _localize(cx.KindKey);;

                    case IdentityStyle.Summary:
                        return cx.Summary;

                    case IdentityStyle.Description:
                        return null;
                }
            }
            return $"{BlankName} {item.IdKey.ToString()}";
        }
        #endregion

        #region ComputeX_Identity  ============================================
        private string ComputeX_Identity(Item item, IdentityStyle style)
        {
            if (item is ComputeX cx)
            {
                var name1 = cx.Name;
                var noName1 = string.IsNullOrWhiteSpace(name1);
                if (noName1) name1 = $"#{cx.Index}";

                var name2 = (Store_ComputeX.TryGetParent(cx, out Store pa)) ? GetIdentity(pa, IdentityStyle.Single) : null;
                var noName2 = string.IsNullOrWhiteSpace(name2);
                if (noName2) name2 = $"{_localize(GetKindKey(IdKey.TableX))} {BlankName}";

                switch (style)
                {
                    case IdentityStyle.Single:
                        return $"{name1}";

                    case IdentityStyle.Double:
                    case IdentityStyle.StoreItem:
                        return $"{name2} : {name1}";

                    case IdentityStyle.ChangeLog:
                        return $"{_localize(cx.KindKey)}: {name2} : {name1}";

                    case IdentityStyle.Kind:
                        return _localize(cx.KindKey);;

                    case IdentityStyle.Summary:
                        return cx.Summary;

                    case IdentityStyle.Description:
                        return null;
                }
            }
            return $"{BlankName} {item.IdKey.ToString()}";
        }
        #endregion

        #region RelationX_Identity  ===========================================
        private string RelationX_Identity(Item item, IdentityStyle style)
        {
            if (item is RelationXO itm)
            {
                var name = string.IsNullOrWhiteSpace(itm.Name) ? string.Empty : $"({itm.Name})";
                if (string.IsNullOrWhiteSpace(name)) name = $"{_localize(itm.KindKey)} {Index_Identity(itm)}";

                var child = (TableX_ParentRelationX.TryGetParent(itm, out TableX ch)) ? GetIdentity(ch, IdentityStyle.Single) : BlankName;
                var parent = (TableX_ChildRelationX.TryGetParent(itm, out TableX pa)) ? GetIdentity(pa, IdentityStyle.Single) : BlankName;

                switch (style)
                {
                    case IdentityStyle.Single:
                    case IdentityStyle.StoreItem:
                        return $"{parent} --> {child}    {name}";

                    case IdentityStyle.Double:
                    case IdentityStyle.ChangeLog:
                        return $"{parent} --> {child}    ({name})";

                    case IdentityStyle.Kind:
                        return _localize(itm.KindKey);

                    case IdentityStyle.Summary:
                        return itm.Summary;

                    case IdentityStyle.Description:
                        return null;
                }
            }
            return $"{BlankName} {item.IdKey.ToString()}";
        }
        #endregion

        #region Relation_Identity  ============================================
        private string Relation_Identity(Item item, IdentityStyle style)
        {
            if (item is Relation itm)
            {
                var kind = _localize(GetKindKey(IdKey.Relation));
                var name = _localize(itm.NameKey);

                var parts = name.Split("_".ToCharArray());
                var child = parts[1];
                var parent = parts[0];

                switch (style)
                {
                    case IdentityStyle.Single:
                    case IdentityStyle.StoreItem:
                        return $"{parent} --> {child}";

                    case IdentityStyle.Double:
                    case IdentityStyle.ChangeLog:
                        return $"{kind} {Index_Identity(itm)}:  {parent} --> {child}";

                    case IdentityStyle.Kind:
                        return kind;

                    case IdentityStyle.Summary:
                        return _localize(itm.SummaryKey);

                    case IdentityStyle.Description:
                        return _localize(itm.DescriptionKey);
                }
            }
            return $"{BlankName} {item.IdKey.ToString()}";
        }
        #endregion

        #region Property_Identity  ============================================
        private string Property_Identity(Item item, IdentityStyle style)
        {
            if (item is Property itm)
            {
                var kind = _localize(GetKindKey(IdKey.Property));
                var name = _localize(itm.NameKey);

                if (!Store_Property.TryGetParent(itm, out Store pa))
                    pa = item.Owner as Store;
                var parent = (pa == null) ? BlankName : _localize(pa.NameKey);

                switch (style)
                {
                    case IdentityStyle.Single:
                        return name;

                    case IdentityStyle.Double:
                    case IdentityStyle.StoreItem:
                        return $"{parent} : {name}";

                    case IdentityStyle.ChangeLog:
                        return $"{kind} {Index_Identity(itm)}: {parent} : {name}";

                    case IdentityStyle.Kind:
                        return kind;

                    case IdentityStyle.Summary:
                        return _localize(itm.SummaryKey);

                    case IdentityStyle.Description:
                        return _localize(itm.DescriptionKey);
                }
            }
            return $"{BlankName} {item.IdKey.ToString()}";
        }
        #endregion

        #region Store_Identity  ===============================================
        private string Store_Identity(Item item, IdentityStyle style)
        {
            if (item is Store itm)
            {
                var kind = _localize(item.KindKey);
                var name = _localize(itm.NameKey);

                switch (style)
                {
                    case IdentityStyle.Single:
                    case IdentityStyle.Double:
                    case IdentityStyle.StoreItem:
                    case IdentityStyle.ChangeLog:
                        return name;

                    case IdentityStyle.Kind:
                        return kind;

                    case IdentityStyle.Summary:
                        return _localize(itm.SummaryKey);

                    case IdentityStyle.Description:
                        return _localize(itm.DescriptionKey);
                }
            }
            return $"{BlankName} {item.IdKey.ToString()}";
        }
        #endregion


        #region Legacy  =======================================================
        internal string GetRowName(RowX row)
        {
            if (TableX_NameProperty.TryGetChild(row.Owner, out Property prop))
            {
                return prop.Value.GetString(row);
            }
            var tbl = row.Owner as TableX;
            var inx = tbl.IndexOf(row);
            return $"#{inx.ToString()}";
        }
        internal string RowSummary(RowX row)
        {
            if (TableX_SummaryProperty.TryGetChild(row.Owner, out Property prop))
            {
                return prop.Value.GetString(row);
            }
            return null;
        }

        #endregion

        #region GetName  ======================================================
        internal string GetKind(IdKey idKe)
        {
            return _localize(GetKindKey(idKe));
        }
        internal string GetName(IdKey idKe)
        {
            return _localize(GetNameKey(idKe));
        }
        internal string GetSummary(IdKey idKe)
        {
            return _localize(GetSummaryKey(idKe));
        }
        internal string GetDescription(IdKey idKe)
        {
            return _localize(GetDescriptionKey(idKe));
        }
        internal string GetAccelerator(IdKey idKe)
        {
            return _localize(GetAcceleratorKey(idKe));
        }
        #endregion
    }

    #region IdentityStyle  ====================================================
    public enum IdentityStyle
    {
        Kind,
        Single,
        Double,
        Summary,
        StoreItem,
        ChangeLog,
        Description,
    }
    #endregion
}
