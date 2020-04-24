using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class QueryXDomain : ExternalDomainOf<QueryX>, ISerializer
    {
        static Guid _serializerGuid = new Guid("33B9B8A4-9332-4902-A3C1-37C5F971B6FF");
        static byte _formatVersion = 1;

        internal PropertyOf<QueryX, string> RootWhereProperty;

        internal PropertyOf<QueryX, string> RelationProperty;
        internal PropertyOf<QueryX, bool> IsReversedProperty;
        internal PropertyOf<QueryX, bool> IsBreakPointProperty;
        internal PropertyOf<QueryX, byte> ExclusiveKeyProperty;
        internal PropertyOf<QueryX, string> WhereProperty;
        internal PropertyOf<QueryX, string> SelectProperty;
        internal PropertyOf<QueryX, string> ValueTypeProperty;
        internal PropertyOf<QueryX, string> LineStyleProperty;
        internal PropertyOf<QueryX, string> DashStyleProperty;
        internal PropertyOf<QueryX, string> LineColorProperty;

        internal PropertyOf<QueryX, string> Facet1Property;
        internal PropertyOf<QueryX, string> Connect1Property;

        internal PropertyOf<QueryX, string> Facet2Property;
        internal PropertyOf<QueryX, string> Connect2Property;

        internal QueryXDomain(Chef chef) : base(chef, IdKey.QueryXStore)
        {
            chef.RegisterItemSerializer((_serializerGuid, this));
            CreateProperties(chef);
        }

        #region CreateProperties  =============================================
        private void CreateProperties(Chef chef)
        {
            var props = new List<Property>(15);
            {
                var p = RootWhereProperty = new PropertyOf<QueryX, string>(chef.PropertyDomain, IdKey.QueryXSelectProperty);
                p.GetValFunc = (item) => p.Cast(item).WhereString;
                p.SetValFunc = (item, value) => chef.TrySetWhereProperty(p.Cast(item), value);
                p.Value = new StringValue(p);
                p.GetItemNameFunc = (item) => { return chef.GetWhereName(p.Cast(item)); };
                props.Add(p);
            }
            {
                var p = Facet1Property = new PropertyOf<QueryX, string>(chef.PropertyDomain, IdKey.QueryXFacet1Property, chef.FacetEnum);
                p.GetValFunc = (item) => chef.GetEnumZName(p.EnumZ, (int)p.Cast(item).PathParm.Facet1);
                p.SetValFunc = (item, value) => { p.Cast(item).PathParm.Facet1 = (Facet)chef.GetEnumZKey(p.EnumZ, value); return chef.RefreshGraphX(p.Cast(item)); };
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = Connect1Property = new PropertyOf<QueryX, string>(chef.PropertyDomain, IdKey.QueryXConnect1Property);
                p.GetValFunc = (item) => chef.GetTargetString(p.Cast(item).PathParm.Target1);
                p.SetValFunc = (item, value) => { p.Cast(item).PathParm.Target1 = chef.GetTargetValue(value); return chef.RefreshGraphX(p.Cast(item)); ; };
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = Facet2Property = new PropertyOf<QueryX, string>(chef.PropertyDomain, IdKey.QueryXFacet2Property, chef.FacetEnum);
                p.GetValFunc = (item) => chef.GetEnumZName(p.EnumZ, (int)p.Cast(item).PathParm.Facet2);
                p.SetValFunc = (item, value) => { p.Cast(item).PathParm.Facet2 = (Facet)chef.GetEnumZKey(p.EnumZ, value); return chef.RefreshGraphX(p.Cast(item)); ; };
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = Connect2Property = new PropertyOf<QueryX, string>(chef.PropertyDomain, IdKey.QueryXConnect2Property);
                p.GetValFunc = (item) => chef.GetTargetString(p.Cast(item).PathParm.Target2);
                p.SetValFunc = (item, value) => { p.Cast(item).PathParm.Target2 = chef.GetTargetValue(value); return chef.RefreshGraphX(p.Cast(item)); ; };
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = LineStyleProperty = new PropertyOf<QueryX, string>(chef.PropertyDomain, IdKey.QueryXLineStyleProperty, chef.LineStyleEnum);
                p.GetValFunc = (item) => chef.GetEnumZName(p.EnumZ, (int)p.Cast(item).PathParm.LineStyle);
                p.SetValFunc = (item, value) => { p.Cast(item).PathParm.LineStyle = (LineStyle)chef.GetEnumZKey(p.EnumZ, value); return chef.RefreshGraphX(p.Cast(item)); ; };
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = DashStyleProperty = new PropertyOf<QueryX, string>(chef.PropertyDomain, IdKey.QueryXDashStyleProperty, chef.DashStyleEnum);
                p.GetValFunc = (item) => chef.GetEnumZName(p.EnumZ, (int)p.Cast(item).PathParm.DashStyle);
                p.SetValFunc = (item, value) => { p.Cast(item).PathParm.DashStyle = (DashStyle)chef.GetEnumZKey(p.EnumZ, value); return chef.RefreshGraphX(p.Cast(item)); ; };
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = LineColorProperty = new PropertyOf<QueryX, string>(chef.PropertyDomain, IdKey.QueryXLineColorProperty);
                p.GetValFunc = (item) => p.Cast(item).PathParm.LineColor;
                p.SetValFunc = (item, value) => { p.Cast(item).PathParm.LineColor = value; return chef.RefreshGraphX(p.Cast(item)); ; };
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = RelationProperty = new PropertyOf<QueryX, string>(chef.PropertyDomain, IdKey.QueryXRelationProperty);
                p.GetValFunc = (item) => chef.GetQueryXRelationName(p.Cast(item));
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = IsReversedProperty = new PropertyOf<QueryX, bool>(chef.PropertyDomain, IdKey.QueryXIsReversedProperty);
                p.GetValFunc = (item) => p.Cast(item).IsReversed;
                p.SetValFunc = (item, value) => { p.Cast(item).IsReversed = value; return true; };
                p.Value = new BoolValue(p);
                props.Add(p);
            }
            {
                var p = IsBreakPointProperty = new PropertyOf<QueryX, bool>(chef.PropertyDomain, IdKey.QueryXIsBreakPointProperty);
                p.GetValFunc = (item) => p.Cast(item).IsBreakPoint;
                p.SetValFunc = (item, value) => { p.Cast(item).IsBreakPoint = value; return true; };
                p.Value = new BoolValue(p);
                props.Add(p);
            }
            {
                var p = ExclusiveKeyProperty = new PropertyOf<QueryX, byte>(chef.PropertyDomain, IdKey.QueryXExclusiveKeyProperty);
                p.GetValFunc = (item) => p.Cast(item).ExclusiveKey;
                p.SetValFunc = (item, value) => { p.Cast(item).ExclusiveKey = (byte)value; return true; };
                p.Value = new ByteValue(p);
                props.Add(p);
            }
            {
                var p = WhereProperty = new PropertyOf<QueryX, string>(chef.PropertyDomain, IdKey.QueryXWhereProperty);
                p.GetValFunc = (item) => p.Cast(item).WhereString;
                p.SetValFunc = (item, value) => chef.TrySetWhereProperty(p.Cast(item), value);
                p.Value = new StringValue(p);
                p.GetItemNameFunc = (item) => { return chef.GetWhereName(p.Cast(item)); };
                props.Add(p);
            }
            {
                var p = SelectProperty = new PropertyOf<QueryX, string>(chef.PropertyDomain, IdKey.ValueXSelectProperty);
                p.GetValFunc = (item) => p.Cast(item).SelectString;
                p.SetValFunc = (item, value) => chef.TrySetSelectProperty(p.Cast(item), value);
                p.Value = new StringValue(p);
                p.GetItemNameFunc = (item) => { return chef.GetSelectName(p.Cast(item)); };
                props.Add(p);
            }
            {
                var p = ValueTypeProperty = new PropertyOf<QueryX, string>(chef.PropertyDomain, IdKey.ValueXValueTypeProperty, chef.ValueTypeEnum);
                p.GetValFunc = (item) => chef.GetEnumZName(p.EnumZ, chef.GetValueType(p.Cast(item)));
                p.Value = new StringValue(p);
                props.Add(p);
            }
            chef.RegisterStaticProperties(typeof(QueryX), props);
        }
        #endregion

        #region ISerializer  ==================================================
        public void ReadData(DataReader r, Item[] items)
        {
            var N = r.ReadInt32();
            if (N < 1) throw new Exception($"Invalid count {N}");
            SetCapacity(N);

            var fv = r.ReadByte();
            if (fv == 1)
            {
                for (int i = 0; i < N; i++)
                {
                    var index = r.ReadInt32();
                    if (index < 0 || index >= items.Length) throw new Exception($"Invalid index {index}");

                    var qx = new QueryX(this);
                    items[index] = qx;

                    var b = r.ReadUInt16();
                    if ((b & S1) != 0) qx.SetState(r.ReadUInt16());
                    if ((b & S2) != 0) qx.WhereString = Value.ReadString(r);
                    if ((b & S3) != 0) qx.SelectString = Value.ReadString(r);
                    if ((b & S4) != 0) qx.ExclusiveKey = r.ReadByte();

                    if (qx.QueryKind == QueryType.Path && qx.IsHead) qx.PathParm = new PathParm();

                    if ((b & S5) != 0) qx.PathParm.Facet1 = (Facet)r.ReadByte();
                    if ((b & S6) != 0) qx.PathParm.Target1 = (Target)r.ReadUInt16();

                    if ((b & S7) != 0) qx.PathParm.Facet2 = (Facet)r.ReadByte();
                    if ((b & S8) != 0) qx.PathParm.Target2 = (Target)r.ReadUInt16();

                    if ((b & S9) != 0) qx.PathParm.DashStyle = (DashStyle)r.ReadByte();
                    if ((b & S10) != 0) qx.PathParm.LineStyle = (LineStyle)r.ReadByte();
                    if ((b & S11) != 0) qx.PathParm.LineColor = Value.ReadString(r);
                }
            }
            else
                throw new Exception($"ColumnXDomain ReadData, unknown format version: {fv}");
        }

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            if (Count > 0)
            {
                w.WriteInt32(Count);
                w.WriteByte(_formatVersion);

                foreach (var qx in Items)
                {
                    w.WriteInt32(itemIndex[qx]);

                    var b = SZ;
                    if (qx.HasState()) b |= S1;
                    if (!string.IsNullOrWhiteSpace(qx.WhereString)) b |= S2;
                    if (!string.IsNullOrWhiteSpace(qx.SelectString)) b |= S3;
                    if (qx.IsExclusive) b |= S4;
                    if (qx.QueryKind == QueryType.Path && qx.IsHead == true && qx.PathParm != null)
                    {
                        if (qx.PathParm.Facet1 != Facet.None) b |= S5;
                        if (qx.PathParm.Target1 != Target.Any) b |= S6;

                        if (qx.PathParm.Facet2 != Facet.None) b |= S7;
                        if (qx.PathParm.Target2 != Target.Any) b |= S8;

                        if (qx.PathParm.DashStyle != DashStyle.Solid) b |= S9;
                        if (qx.PathParm.LineStyle != LineStyle.PointToPoint) b |= S10;
                        if (!string.IsNullOrWhiteSpace(qx.PathParm.LineColor)) b |= S11;
                    }

                    w.WriteUInt16(b);
                    if ((b & S1) != 0) w.WriteUInt16(qx.GetState());
                    if ((b & S2) != 0) Value.WriteString(w, qx.WhereString);
                    if ((b & S3) != 0) Value.WriteString(w, qx.SelectString);
                    if ((b & S4) != 0) w.WriteByte(qx.ExclusiveKey);

                    if ((b & S5) != 0) w.WriteByte((byte)qx.PathParm.Facet1);
                    if ((b & S6) != 0) w.WriteUInt16((ushort)qx.PathParm.Target1);

                    if ((b & S7) != 0) w.WriteByte((byte)qx.PathParm.Facet2);
                    if ((b & S8) != 0) w.WriteUInt16((ushort)qx.PathParm.Target2);

                    if ((b & S9) != 0) w.WriteByte((byte)qx.PathParm.DashStyle);
                    if ((b & S10) != 0) w.WriteByte((byte)qx.PathParm.LineStyle);
                    if ((b & S11) != 0) Value.WriteString(w, qx.PathParm.LineColor);
                }
            }
        }
        #endregion
    }
}
