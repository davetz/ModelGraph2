using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class EnumXStore : ExternalStoreOf<EnumX>, ISerializer
    {
        static Guid _serializerGuid = new Guid("8D4CEAD8-E3C5-4342-88AC-1B4B625A9A4C");
        static byte _formatVersion = 1;

        internal PropertyOf<EnumX, string> NameProperty;
        internal PropertyOf<EnumX, string> SummaryProperty;

        internal PropertyOf<PairX, string> TextProperty;
        internal PropertyOf<PairX, string> ValueProperty;

        internal EnumXStore(Chef chef) : base(chef, IdKey.EnumXStore, 10)
        {
            chef.RegisterItemSerializer((_serializerGuid, this));
            CreateProperties(chef);
        }

        #region CreateProperties  =============================================
        private void CreateProperties(Chef chef)
        {
            var props1 = new List<Property>(2);
            {
                var p = NameProperty = new PropertyOf<EnumX, string>(chef.PropertyStore, IdKey.EnumNameProperty);
                p.GetValFunc = (item) => p.Cast(item).Name;
                p.SetValFunc = (item, value) => { p.Cast(item).Name = value; return true; };
                p.Value = new StringValue(p);
                props1.Add(p);
            }
            {
                var p = SummaryProperty = new PropertyOf<EnumX, string>(chef.PropertyStore, IdKey.EnumSummaryProperty);
                p.GetValFunc = (item) => p.Cast(item).Summary;
                p.SetValFunc = (item, value) => { p.Cast(item).Summary = value; return true; };
                p.Value = new StringValue(p);
                props1.Add(p);
            }
            chef.RegisterStaticProperties(typeof(EnumX), props1);

            var props2 = new List<Property>(2);
            {
                var p = TextProperty = new PropertyOf<PairX, string>(chef.PropertyZStore, IdKey.EnumTextProperty);
                p.GetValFunc = (item) => p.Cast(item).DisplayValue;
                p.SetValFunc = (item, value) => { p.Cast(item).DisplayValue = value; p.Owner.ChildDelta++; return true; };
                p.Value = new StringValue(p);
                props2.Add(p);
            }
            {
                var p = ValueProperty = new PropertyOf<PairX, string>(chef.PropertyZStore, IdKey.EnumValueProperty);
                p.GetValFunc = (item) => p.Cast(item).ActualValue;
                p.SetValFunc = (item, value) => { p.Cast(item).ActualValue = value; p.Owner.ChildDelta++; return true; };
                p.Value = new StringValue(p);
                props2.Add(p);
            }
            chef.RegisterStaticProperties(typeof(PairX), props2);
        }
        #endregion

        #region ISerializer  ==================================================
        public void ReadData(DataReader r, Item[] items)
        {
            var N = r.ReadInt32();
            if (N < 0) throw new Exception($"Invalid count {N}");
            SetCapacity(N);

            var fv = r.ReadByte();
            if (fv == 1)
            {
                for (int i = 0; i < N; i++)
                {
                    var index = r.ReadInt32();
                    if (index < 0 || index >= items.Length) throw new Exception($"Invalid index {index}");

                    var ex = new EnumX(this);
                    items[index] = ex;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) ex.SetState(r.ReadUInt16());
                    if ((b & B2) != 0) ex.Name = Value.ReadString(r);
                    if ((b & B3) != 0) ex.Summary = Value.ReadString(r);
                    if ((b & B4) != 0) ex.Description = Value.ReadString(r);

                    var pxCount = r.ReadByte();
                    if (pxCount > 0) ex.SetCapacity(pxCount);

                    for (int j = 0; j < pxCount; j++)
                    {
                        var index2 = r.ReadInt32();
                        if (index2 < 0 || index2 >= items.Length) throw new Exception($"Invalid value index {index2}");

                        var px = new PairX(ex);
                        items[index2] = px;

                        px.ActualValue = Value.ReadString(r);
                        px.DisplayValue = Value.ReadString(r);
                    }
                }
            }
            else
                throw new Exception($"EnumXStore ReadData, unknown format version: {fv}");
        }

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            if (Count > 0)
            {
                w.WriteInt32(Count);
                w.WriteByte(_formatVersion);

                foreach (var ex in Items)
                {
                    w.WriteInt32(itemIndex[ex]);

                    var b = BZ;
                    if (ex.HasState()) b |= B1;
                    if (!string.IsNullOrWhiteSpace(ex.Name)) b |= B2;
                    if (!string.IsNullOrWhiteSpace(ex.Summary)) b |= B3;
                    if (!string.IsNullOrWhiteSpace(ex.Description)) b |= B4;

                    w.WriteByte(b);
                    if ((b & B1) != 0) w.WriteUInt16(ex.GetState());
                    if ((b & B2) != 0) Value.WriteString(w, ex.Name);
                    if ((b & B3) != 0) Value.WriteString(w, ex.Summary);
                    if ((b & B4) != 0) Value.WriteString(w, ex.Description);

                    if (ex.Count > 0 && ex.Count < byte.MaxValue)
                    {
                        w.WriteByte((byte)ex.Count);

                        foreach (var px in ex.Items)
                        {
                            w.WriteInt32(itemIndex[px]);

                            Value.WriteString(w, string.IsNullOrWhiteSpace(px.ActualValue) ? "0" : px.ActualValue);
                            Value.WriteString(w, string.IsNullOrWhiteSpace(px.DisplayValue) ? "?" : px.DisplayValue);
                        }
                    }
                    else
                    {
                        w.WriteByte((byte)0);
                    }
                }
            }
        }
        #endregion
    }
}
