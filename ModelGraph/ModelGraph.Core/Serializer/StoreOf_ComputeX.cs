using System;
using System.Collections.Generic;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class StoreOf_ComputeX : StoreOf_External<ComputeX>, ISerializer
    {
        static Guid _serializerGuid = new Guid("35522B27-A925-4CE0-8D65-EDEF451097F2");
        static byte _formatVersion = 1;
        internal override IdKey IdKey => IdKey.ComputeXDomain;

        internal StoreOf_ComputeX(Chef chef)
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

            chef.RegisterReferenceItem(new Property_ComputeX_CompuType(sto));
            chef.RegisterReferenceItem(new Property_ComputeX_Where(sto));
            chef.RegisterReferenceItem(new Property_ComputeX_Select(sto));
            chef.RegisterReferenceItem(new Property_ComputeX_Separator(sto));
            chef.RegisterReferenceItem(new Property_ComputeX_ValueType(sto));

            chef.RegisterStaticProperties(typeof(ComputeX), GetProps(chef)); //used by property name lookup
        }
        private Property[] GetProps(Chef chef) => new Property[]
        {
            chef.Get<Property_Item_Name>(),
            chef.Get<Property_Item_Summary>(),
            chef.Get<Property_Item_Description>(),

            chef.Get<Property_ComputeX_CompuType>(),
            chef.Get<Property_ComputeX_Where>(),
            chef.Get<Property_ComputeX_Select>(),
            chef.Get<Property_ComputeX_Separator>(),
            chef.Get<Property_ColumnX_ValueType>(),
        };
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
