using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class TableXStore : ExternalStore<TableX>, ISerializer
    {
        static Guid _serializerGuid = new Guid("93EC136C-6C38-474D-844B-6B8326526CB5");
        static byte _formatVersion = 1;

        internal TableXStore(Chef owner) : base(owner, Trait.ViewXStore, 30)
        {
            owner.RegisterSerializer((_serializerGuid, this));
        }

        #region ISerializer  ==================================================
        public void ReadData(DataReader r, Item[] items)
        {
            var N = r.ReadInt32();
            if (N < 1) throw new Exception($"Invalid count {N}");

            var fv = r.ReadByte();
            if (fv == 1)
            {
                for (int i = 0; i < N; i++)
                {
                    var index = r.ReadInt32();
                    if (index < 0 || index >= items.Length) throw new Exception($"Invalid index {index}");

                    var vx = new ViewX(this);
                    items[index] = vx;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) vx.Name = Value.ReadString(r);
                    if ((b & B2) != 0) vx.Summary = Value.ReadString(r);
                    if ((b & B3) != 0) vx.Description = Value.ReadString(r);
                }
            }
            else
                throw new Exception($"ViewXStore ReadData, unknown format version: {fv}");
        }

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            if (Count > 0)
            {
                w.WriteInt32(Count);
                w.WriteByte(_formatVersion);

                foreach (var view in Items)
                {
                    w.WriteInt32(itemIndex[view]);

                    var b = BZ;
                    if (!string.IsNullOrWhiteSpace(view.Name)) b |= B1;
                    if (!string.IsNullOrWhiteSpace(view.Summary)) b |= B2;
                    if (!string.IsNullOrWhiteSpace(view.Description)) b |= B3;

                    w.WriteByte(b);
                    if ((b & B1) != 0) Value.WriteString(w, view.Name);
                    if ((b & B2) != 0) Value.WriteString(w, view.Summary);
                    if ((b & B3) != 0) Value.WriteString(w, view.Description);
                }
            }
        }
        #endregion
    }
}
