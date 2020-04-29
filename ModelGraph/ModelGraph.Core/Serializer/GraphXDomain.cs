using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class GraphXDomain : ExternalDomainOf<GraphX>, ISerializer
    {
        static Guid _serializerGuid = new Guid("48C7FA8C-88F1-4203-8E54-3255C1F8C528");
        static byte _formatVersion = 1;

        internal PropertyOf<GraphX, string> NameProperty;
        internal PropertyOf<GraphX, string> SummaryProperty;
        internal PropertyOf<GraphX, int> TerminalLengthProperty;
        internal PropertyOf<GraphX, int> TerminalSpacingProperty;
        internal PropertyOf<GraphX, int> TerminalStretchProperty;
        internal PropertyOf<GraphX, int> SymbolSizeProperty;

        internal GraphXDomain(Chef chef) : base(chef, IdKey.GraphXDomain)
        {
            chef.RegisterItemSerializer((_serializerGuid, this));
            CreateProperties(chef);
        }

        #region CreateProperties  =============================================
        private void CreateProperties(Chef chef)
        {
            var props = new List<Property>(6);
            {
                var p = NameProperty = new PropertyOf<GraphX, string>(chef.PropertyDomain, IdKey.GraphNameProperty);
                p.GetValFunc = (item) => p.Cast(item).Name;
                p.SetValFunc = (item, value) => { p.Cast(item).Name = value; return true; };
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = SummaryProperty = new PropertyOf<GraphX, string>(chef.PropertyDomain, IdKey.GraphSummaryProperty);
                p.GetValFunc = (item) => p.Cast(item).Summary;
                p.SetValFunc = (item, value) => { p.Cast(item).Summary = value; return true; };
                p.Value = new StringValue(p);
                props.Add(p);
            }
            {
                var p = TerminalLengthProperty = new PropertyOf<GraphX, int>(chef.PropertyDomain, IdKey.GraphTerminalLengthProperty);
                p.GetValFunc = (item) => p.Cast(item).TerminalLength;
                p.SetValFunc = (item, value) => { p.Cast(item).TerminalLength = (byte)value; return true; };
                p.Value = new Int32Value(p);
                props.Add(p);
            }
            {
                var p = TerminalSpacingProperty = new PropertyOf<GraphX, int>(chef.PropertyDomain, IdKey.GraphTerminalSpacingProperty);
                p.GetValFunc = (item) => p.Cast(item).TerminalSpacing;
                p.SetValFunc = (item, value) => { p.Cast(item).TerminalSpacing = (byte)value; return true; };
                p.Value = new Int32Value(p);
                props.Add(p);
            }
            {
                var p = TerminalStretchProperty = new PropertyOf<GraphX, int>(chef.PropertyDomain, IdKey.GraphTerminalStretchProperty);
                p.GetValFunc = (item) => p.Cast(item).TerminalSkew;
                p.SetValFunc = (item, value) => { p.Cast(item).TerminalSkew = (byte)value; return true; };
                p.Value = new Int32Value(p);
                props.Add(p);
            }
            {
                var p = SymbolSizeProperty = new PropertyOf<GraphX, int>(chef.PropertyDomain, IdKey.GraphSymbolSizeProperty);
                p.GetValFunc = (item) => p.Cast(item).SymbolSize;
                p.SetValFunc = (item, value) => { p.Cast(item).SymbolSize = (byte)value; return true; };
                p.Value = new Int32Value(p);
                props.Add(p);
            }
            chef.RegisterStaticProperties(typeof(GraphX), props);
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

                    var gx = new GraphX(this);
                    items[index] = gx;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) gx.SetState(r.ReadUInt16());
                    if ((b & B2) != 0) gx.Name = Value.ReadString(r);
                    if ((b & B3) != 0) gx.Summary = Value.ReadString(r);
                    if ((b & B4) != 0) gx.Description = Value.ReadString(r);

                    gx.MinNodeSize = r.ReadByte();
                    gx.ThinBusSize = r.ReadByte();
                    gx.WideBusSize = r.ReadByte();
                    gx.ExtraBusSize = r.ReadByte();

                    gx.SurfaceSkew = r.ReadByte();
                    gx.TerminalSkew = r.ReadByte();
                    gx.TerminalLength = r.ReadByte();
                    gx.TerminalSpacing = r.ReadByte();
                    gx.SymbolSize = r.ReadByte();
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

                foreach (var gx in Items)
                {
                    w.WriteInt32(itemIndex[gx]);

                    var b = BZ;
                    if (gx.HasState()) b |= B1;
                    if (!string.IsNullOrWhiteSpace(gx.Name)) b |= B2;
                    if (!string.IsNullOrWhiteSpace(gx.Summary)) b |= B3;
                    if (!string.IsNullOrWhiteSpace(gx.Description)) b |= B4;

                    w.WriteByte(b);
                    if ((b & B1) != 0) w.WriteUInt16(gx.GetState());
                    if ((b & B2) != 0) Value.WriteString(w, gx.Name);
                    if ((b & B3) != 0) Value.WriteString(w, gx.Summary);
                    if ((b & B4) != 0) Value.WriteString(w, gx.Description);

                    w.WriteByte(gx.MinNodeSize);
                    w.WriteByte(gx.ThinBusSize);
                    w.WriteByte(gx.WideBusSize);
                    w.WriteByte(gx.ExtraBusSize);

                    w.WriteByte(gx.SurfaceSkew);
                    w.WriteByte(gx.TerminalSkew);
                    w.WriteByte(gx.TerminalLength);
                    w.WriteByte(gx.TerminalSpacing);
                    w.WriteByte(gx.SymbolSize);
                }
            }
        }
        #endregion
    }
}
