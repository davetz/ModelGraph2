using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class TableXStore : ExternalStoreOf<TableX>, ISerializer, IPropertyManager
    {
        static Guid _serializerGuid = new Guid("93EC136C-6C38-474D-844B-6B8326526CB5");
        static byte _formatVersion = 1;

        internal PropertyOf<TableX, string> NameProperty;
        internal PropertyOf<TableX, string> SummaryProperty;

        internal TableXStore(Chef chef) : base(chef, Trait.TableXStore, 30)
        {
            chef.RegisterItemSerializer((_serializerGuid, this));
            CreateProperties(chef);
        }

        #region CreateProperties  =============================================
        private void CreateProperties(Chef chef)
        {
            {
                var p = NameProperty = new PropertyOf<TableX, string>(chef.PropertyStore, Trait.TableName_P);
                p.GetValFunc = (item) => p.Cast(item).Name;
                p.SetValFunc = (item, value) => { p.Cast(item).Name = value; return true; };
                p.Value = new StringValue(p);
            }
            {
                var p = SummaryProperty = new PropertyOf<TableX, string>(chef.PropertyStore, Trait.TableSummary_P);
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

                    var tx = new TableX(this);
                    items[index] = tx;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) tx.SetState(r.ReadUInt16());
                    if ((b & B2) != 0) tx.Name = Value.ReadString(r);
                    if ((b & B3) != 0) tx.Summary = Value.ReadString(r);
                    if ((b & B4) != 0) tx.Description = Value.ReadString(r);

                    var rxCount = r.ReadInt32();
                    if (rxCount < 0) throw new Exception($"Invalid row count {rxCount}");
                    if (rxCount > 0) tx.SetCapacity(rxCount);

                    for (int j = 0; j < rxCount; j++)
                    {
                        var index2 = r.ReadInt32();
                        if (index2 < 0 || index2 >= items.Length) throw new Exception($"Invalid row index {index2}");

                        items[index2] = new RowX(tx);
                    }
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

                foreach (var tx in Items)
                {
                    w.WriteInt32(itemIndex[tx]);

                    var b = BZ;
                    if (tx.HasState()) b |= B1;
                    if (!string.IsNullOrWhiteSpace(tx.Name)) b |= B2;
                    if (!string.IsNullOrWhiteSpace(tx.Summary)) b |= B3;
                    if (!string.IsNullOrWhiteSpace(tx.Description)) b |= B4;

                    w.WriteByte(b);
                    if ((b & B1) != 0) w.WriteUInt16(tx.GetState());
                    if ((b & B2) != 0) Value.WriteString(w, tx.Name);
                    if ((b & B3) != 0) Value.WriteString(w, tx.Summary);
                    if ((b & B4) != 0) Value.WriteString(w, tx.Description);

                    if (tx.Count > 0)
                    {
                        w.WriteInt32(tx.Count);
                        foreach (var rx in tx.Items)
                        {
                            w.WriteInt32(itemIndex[rx]);
                        }
                    }
                    else
                    {
                        w.WriteInt32(0);
                    }
                }
            }
        }
        #endregion
    }
}
