using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class ViewXStore : ExternalStoreOf<ViewX>, ISerializer, IPropertyManager
    {
        static Guid _serializerGuid = new Guid("396EC955-832E-4BEA-9E5C-C2A203ADAD23");
        static byte _formatVersion = 1;

        internal PropertyOf<ViewX, string> NameProperty;
        internal PropertyOf<ViewX, string> SummaryProperty;

        internal ViewXStore(Chef chef) : base(chef, Trait.ViewXStore, 30)
        {
            chef.RegisterItemSerializer((_serializerGuid, this));
            CreateProperties(chef);
        }

        #region CreateProperties  =============================================
        private void CreateProperties(Chef chef)
        {
            {
                var p = NameProperty = new PropertyOf<ViewX, string>(chef.PropertyStore, Trait.ViewName_P);
                p.GetValFunc = (item) => p.Cast(item).Name;
                p.SetValFunc = (item, value) => { p.Cast(item).Name = value; return true; };
                p.Value = new StringValue(p);
            }
            {
                var p = SummaryProperty = new PropertyOf<ViewX, string>(chef.PropertyStore, Trait.ViewSummary_P);
                p.GetValFunc = (item) => p.Cast(item).Summary;
                p.SetValFunc = (item, value) => { p.Cast(item).Summary = value; return true; };
                p.Value = new StringValue(p);
            }
        }
        #endregion

        #region IPropertyManager  =============================================
        public Property[] GetPropreties(ItemModel model = null) => new Property[]
        {
            NameProperty,
            SummaryProperty,
        };
        #endregion

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
