using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class StoreOf_ColumnX : StoreOf_External<ColumnX>, ISerializer
    {
        static Guid _serializerGuid = new Guid("3E7097FE-22D5-43B2-964A-9DB843F6D55B");
        static byte _formatVersion = 1;
        internal override IdKey ViKey => IdKey.ColumnXDomain;

        internal StoreOf_ColumnX(Chef chef)
        {
            Owner = chef;

            chef.RegisterItemSerializer((_serializerGuid, this));
            CreateProperties(chef);

            chef.Add(this);
        }

        #region CreateProperties  =============================================
        private void CreateProperties(Chef chef)
        {
            var sto = chef.Get<StoreOf_Property>();

            chef.RegisterReferenceItem(new Property_ColumnX_ValueType(sto));
            chef.RegisterReferenceItem(new Property_ColumnX_IsChoice(sto));

            chef.RegisterStaticProperties(typeof(ColumnX), GetProps(chef)); //used by property name lookup
        }
        private Property[] GetProps(Chef chef) => new Property[]
        {
            chef.Get<Property_Item_Name>(),
            chef.Get<Property_Item_Summary>(),
            chef.Get<Property_Item_Description>(),

            chef.Get<Property_ColumnX_ValueType>(),
            chef.Get<Property_ColumnX_IsChoice>(),
        };
        #endregion

        #region ISerializer  ==================================================
        #region ReadData  =====================================================
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

                    var cx = new ColumnX(this);
                    items[index] = cx;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) cx.SetState(r.ReadUInt16());
                    if ((b & B2) != 0) cx.Name = Value.ReadString(r);
                    if ((b & B3) != 0) cx.Summary = Value.ReadString(r);
                    if ((b & B4) != 0) cx.Description = Value.ReadString(r);

                    ValueDictionary.ReadData(r, cx, items);
                }
            }
            else
                throw new Exception($"ColumnXDomain ReadData, unknown format version: {fv}");
        }
        #endregion

        #region WriteData  ====================================================
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

                    w.WriteByte(b);
                    if ((b & B1) != 0) w.WriteUInt16(cx.GetState());
                    if ((b & B2) != 0) Value.WriteString(w, cx.Name);
                    if ((b & B3) != 0) Value.WriteString(w, cx.Summary);
                    if ((b & B4) != 0) Value.WriteString(w, cx.Description);

                    cx.Value.WriteData(w, itemIndex);
                }
            }
        }
        #endregion
        #endregion
    }
}
