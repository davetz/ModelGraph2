using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class ViewXStore : StoreOf<ViewX>, ISerializer
    {
        static Guid _serializerGuid = new Guid("396EC955-832E-4BEA-9E5C-C2A203ADAD23");
        static Guid _internalItemGuid = new Guid("C11EAF6E-20A2-4F2E-AF19-0BC49DF561AB");

        internal ViewXStore(Chef owner) : base(owner, Trait.ViewXStore, 30)
        {
            owner.RegisterReference(this, _internalItemGuid);
            owner.RegisterSerializer((_serializerGuid, this));
        }

        #region ISerializer  ==================================================
        public bool HasData() => Count > 0;

        public void ReadData(DataReader r, Item[] items)
        {
            var N = r.ReadInt32();
            if (N < 0) throw new Exception($"Invalid count {N}");

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

        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            if (Count > 0)
            {
                w.WriteInt32(Count);

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
