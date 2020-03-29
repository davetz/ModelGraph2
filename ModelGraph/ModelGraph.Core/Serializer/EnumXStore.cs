using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class EnumXStore : ExternalStore<EnumX>, ISerializer
    {
        static Guid _serializerGuid = new Guid("8D4CEAD8-E3C5-4342-88AC-1B4B625A9A4C");
        static byte _formatVersion = 1;

        internal EnumXStore(Chef owner) : base(owner, Trait.EnumXStore, 10)
        {
            owner.RegisterItemSerializer((_serializerGuid, this));
        }

        #region ISerializer  ==================================================
        public void ReadData(DataReader r, Item[] items)
        {
            var N = r.ReadInt32();
            if (N < 0) throw new Exception($"Invalid count {N}");

            for (int i = 0; i < N; i++)
            {
                var index = r.ReadInt32();
                if (index < 0 || index >= items.Length) throw new Exception($"Invalid index {index}");

                var ex = new EnumX(this);
                items[index] = ex;

                var b = r.ReadByte();
                if ((b & B1) != 0) ex.Name = Value.ReadString(r);
                if ((b & B2) != 0) ex.Summary = Value.ReadString(r);
                if ((b & B3) != 0) ex.Description = Value.ReadString(r);

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

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            if (Count > 0)
            {
                w.WriteInt32(Count);

                foreach (var ex in Items)
                {
                    w.WriteInt32(itemIndex[ex]);

                    var b = BZ;
                    if (!string.IsNullOrWhiteSpace(ex.Name)) b |= B1;
                    if (!string.IsNullOrWhiteSpace(ex.Summary)) b |= B2;
                    if (!string.IsNullOrWhiteSpace(ex.Description)) b |= B3;

                    w.WriteByte(b);
                    if ((b & B1) != 0) Value.WriteString(w, ex.Name);
                    if ((b & B2) != 0) Value.WriteString(w, ex.Summary);
                    if ((b & B3) != 0) Value.WriteString(w, ex.Description);

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
