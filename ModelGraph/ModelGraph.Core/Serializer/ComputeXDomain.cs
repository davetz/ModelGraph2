using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class ComputeXDomain : ExternalDomainOf<ComputeX>, ISerializer
    {
        static Guid _serializerGuid = new Guid("35522B27-A925-4CE0-8D65-EDEF451097F2");
        static byte _formatVersion = 1;

        internal PropertyOf<ComputeX, string> NameProperty;
        internal PropertyOf<ComputeX, string> SummaryProperty;
        internal PropertyOf<ComputeX, string> WhereProperty;
        internal PropertyOf<ComputeX, string> SelectProperty;
        internal PropertyOf<ComputeX, string> SeparatorProperty;
        internal PropertyOf<ComputeX, string> CompuTypeProperty;
        internal PropertyOf<ComputeX, string> ValueTypeProperty;

        internal ComputeXDomain(Chef chef) : base(chef, IdKey.ComputeXDomain)
        {
            chef.RegisterItemSerializer((_serializerGuid, this));
            CreateProperties(chef);
        }

        #region CreateProperties  =============================================
        private void CreateProperties(Chef chef)
        {
            var props = new List<Property>(7);
            var propertyStore = chef.PropertyDomain;
            {
                var p = NameProperty = new PropertyOf<ComputeX, string>(propertyStore, IdKey.ComputeXNameProperty);
                p.GetValFunc = (item) => p.Cast(item).Name;
                p.SetValFunc = (item, value) => { p.Cast(item).Name = value; return true; };
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = SummaryProperty = new PropertyOf<ComputeX, string>(propertyStore, IdKey.ComputeXSummaryProperty);
                p.GetValFunc = (item) => p.Cast(item).Summary;
                p.SetValFunc = (item, value) => { p.Cast(item).Summary = value; return true; };
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = CompuTypeProperty = new PropertyOf<ComputeX, string>(propertyStore, IdKey.ComputeXCompuTypeProperty, chef.ComputeTypeEnum);
                p.GetValFunc = (item) => chef.GetEnumZName(p.EnumZ, (int)p.Cast(item).CompuType);
                p.SetValFunc = (item, value) => chef.TrySetComputeTypeProperty(p.Cast(item), chef.GetEnumZKey(p.EnumZ, value));
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = WhereProperty = new PropertyOf<ComputeX, string>(propertyStore, IdKey.ComputeXWhereProperty);
                p.GetValFunc = (item) => chef.GetWhereProperty(p.Cast(item));
                p.SetValFunc = (item, value) => chef.TrySetWhereProperty(p.Cast(item), value);
                p.Value = new StringValue(p);
                p.GetItemNameFunc = (item) => chef.GetSelectorName(p.Cast(item));
                props.Add(p);
            }
            {
                var p = SelectProperty = new PropertyOf<ComputeX, string>(propertyStore, IdKey.ComputeXSelectProperty);
                p.GetValFunc = (item) => chef.GetSelectProperty(p.Cast(item));
                p.SetValFunc = (item, value) => chef.TrySetSelectProperty(p.Cast(item), value);
                p.Value = new StringValue(p);
                p.GetItemNameFunc = (item) => { return chef.GetSelectorName(p.Cast(item)); };
                props.Add(p);
            }
            {
                var p = SeparatorProperty = new PropertyOf<ComputeX, string>(propertyStore, IdKey.ComputeXSeparatorProperty);
                p.GetValFunc = (item) => p.Cast(item).Separator;
                p.SetValFunc = (item, value) => { p.Cast(item).Separator = value; return true; };
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = ValueTypeProperty = new PropertyOf<ComputeX, string>(propertyStore, IdKey.ComputeXValueTypeProperty, chef.ValueTypeEnum);
                p.GetValFunc = (item) => chef.GetEnumZName(p.EnumZ, (int)p.Cast(item).Value.ValType);
                p.Value = new StringValue(p);
                props.Add(p);
            }
            chef.RegisterStaticProperties(typeof(ComputeX), props);
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

                    var cx = new ComputeX(this);
                    items[index] = cx;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) cx.SetState(r.ReadUInt16());
                    if ((b & B2) != 0) cx.Name = Value.ReadString(r);
                    if ((b & B3) != 0) cx.Summary = Value.ReadString(r);
                    if ((b & B4) != 0) cx.Description = Value.ReadString(r);
                    if ((b & B5) != 0) cx.Separator = Value.ReadString(r);
                    if ((b & B6) != 0) cx.CompuType = (CompuType)r.ReadByte();
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

                foreach (var cx in Items)
                {
                    w.WriteInt32(itemIndex[cx]);

                    var b = BZ;
                    if (cx.HasState()) b |= B1;
                    if (!string.IsNullOrWhiteSpace(cx.Name)) b |= B2;
                    if (!string.IsNullOrWhiteSpace(cx.Summary)) b |= B3;
                    if (!string.IsNullOrWhiteSpace(cx.Description)) b |= B4;
                    if (cx.Separator != ComputeX.DefaultSeparator) b |= B5;
                    if (cx.CompuType != CompuType.RowValue) b |= B6;

                    w.WriteByte(b);
                    if ((b & B1) != 0) w.WriteUInt16(cx.GetState());
                    if ((b & B2) != 0) Value.WriteString(w, cx.Name);
                    if ((b & B3) != 0) Value.WriteString(w, cx.Summary);
                    if ((b & B4) != 0) Value.WriteString(w, cx.Description);
                    if ((b & B5) != 0) Value.WriteString(w, (cx.Separator ?? string.Empty));
                    if ((b & B6) != 0) w.WriteByte((byte)cx.CompuType);
                }
            }
        }
        #endregion
    }
}
