using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public class StoreOf_RelationX : StoreOf_External<Relation>, ISerializer, IRelationStore
    {
        static Guid _serializerGuid = new Guid("D950F508-B774-4838-B81A-757EFDC40518");
        static byte _formatVersion = 1;
        internal override IdKey ViKey => IdKey.RelationXDomain;

        internal PropertyOf<Relation, string> NameProperty;
        internal PropertyOf<Relation, string> SummaryProperty;
        internal PropertyOf<Relation, string> PairingProperty;
        internal PropertyOf<Relation, bool> IsRequiredProperty;


        #region Constructor  ==================================================
        internal StoreOf_RelationX(Chef chef)
        {
            Owner = chef;

            chef.RegisterItemSerializer((_serializerGuid, this));
            CreateProperties(chef);

            new RelationXLink(chef, this);

            chef.Add(this);
        }
        #endregion


        #region CreateProperties  =============================================
        private void CreateProperties(Chef chef)
        {
            var sto = chef.Get<StoreOf_Property>();

            chef.RegisterReferenceItem(new Property_Relation_Pairing(sto));
            chef.RegisterReferenceItem(new Property_Relation_IsRequired(sto));

            chef.RegisterStaticProperties(typeof(Relation), GetProps(chef)); //used by property name lookup
        }
        private Property[] GetProps(Chef chef) => new Property[]
        {
            chef.Get<Property_Item_Name>(),
            chef.Get<Property_Item_Summary>(),
            chef.Get<Property_Item_Description>(),

            chef.Get<Property_Relation_Pairing>(),
            chef.Get<Property_Relation_IsRequired>(),
        };
        #endregion

        #region IRelationStore  ===============================================
        public Relation[] GetRelationArray()
        {
            var relationArray = new Relation[Count];
            for (int i = 0; i < Count; i++)
            {
                relationArray[i] = Items[i];
            }
            return relationArray;
        }
        #endregion

        #region ISerializer  ==================================================
        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            w.WriteInt32(Count);
            w.WriteByte(_formatVersion);

            foreach (var rx in Items)
            {
                w.WriteInt32(itemIndex[rx]);

                var keyCount = rx.KeyCount;
                var valCount = rx.ValueCount;

                var b = BZ;
                if (rx.HasState()) b |= B1;
                if (!string.IsNullOrWhiteSpace(rx.Name)) b |= B2;
                if (!string.IsNullOrWhiteSpace(rx.Summary)) b |= B3;
                if (!string.IsNullOrWhiteSpace(rx.Description)) b |= B4;
                if (rx.Pairing != Pairing.OneToMany) b |= B5;
                if ((keyCount + valCount) > 0) b |= B6;

                w.WriteByte(b);
                if ((b & B1) != 0) w.WriteUInt16(rx.GetState());
                if ((b & B2) != 0) Value.WriteString(w, rx.Name);
                if ((b & B3) != 0) Value.WriteString(w, rx.Summary);
                if ((b & B4) != 0) Value.WriteString(w, rx.Description);
                if ((b & B5) != 0) w.WriteByte((byte)rx.Pairing);
                if ((b & B6) != 0) w.WriteInt32(keyCount);
                if ((b & B6) != 0) w.WriteInt32(valCount);
            }
        }
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
                    if (index < 0 || index >= items.Length) throw new Exception($"RelationXStore ReadData, invalid index {index}");

                    var rx = new RelationXO(this, true);
                    items[index] = rx;

                    var b = r.ReadByte();
                    if ((b & B1) != 0) rx.SetState(r.ReadUInt16());
                    if ((b & B2) != 0) rx.Name = Value.ReadString(r);
                    if ((b & B3) != 0) rx.Summary = Value.ReadString(r);
                    if ((b & B4) != 0) rx.Description = Value.ReadString(r);
                    if ((b & B5) != 0) rx.Pairing = (Pairing)r.ReadByte();
                    var keyCount = ((b & B6) != 0) ? r.ReadInt32() : 0;
                    var valCount = ((b & B6) != 0) ? r.ReadInt32() : 0;
                    rx.Initialize(keyCount, valCount);
                }
            }
            else
                throw new Exception($"RelationXStore ReadData, unknown format version: {fv}");
        }
        #endregion
    }
}
