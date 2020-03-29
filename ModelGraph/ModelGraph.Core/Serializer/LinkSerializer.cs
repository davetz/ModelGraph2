using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    internal abstract class LinkSerializer
    {
        static byte _formatVersion = 1;
        protected IRelationStore _relationStore;

        internal LinkSerializer(IRelationStore relStore)
        {
            _relationStore = relStore;
        }

        public bool HasData()
        {
            var rlationArray = _relationStore.GetRelationArray();
            foreach (var rel in rlationArray)
            {
                if (rel.HasLinks) return true;
            }
            return false;
        }

        #region WriteData  ====================================================
        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            var rlationArray = _relationStore.GetRelationArray();
            var N = 0;
            foreach (var rel in rlationArray) { if (rel.HasLinks) N++; } //count number of serialized relations 

            w.WriteInt32(N);                //number of serialized relations 
            w.WriteByte(_formatVersion);    //format version

            foreach (var rel in rlationArray)  //foreach relation entry
            {
                if (rel.HasLinks)
                {
                    w.WriteInt32(itemIndex[rel]);    //relation index
                    w.WriteByte((byte)rel.Pairing);  //pairing cross check, it should match the relation.pairing on reading

                    switch (rel.Pairing)
                    {
                        case Pairing.OneToOne:

                            var list1 = rel.GetChildren1Items(itemIndex);

                            w.WriteInt32(list1.Length);   //number of parent/child link pairs in this OntToOne relation
                            foreach (var (ix1, ix2) in list1)
                            {
                                w.WriteInt32(ix1);      //parent item
                                w.WriteInt32(ix2);      //child item
                            }
                            break;

                        case Pairing.OneToMany:

                            var list2 = rel.GetChildren2Items(itemIndex);

                            WriteList(w, list2);   //write the compound list
                            break;

                        case Pairing.ManyToMany:

                            var list3 = rel.GetChildren2Items(itemIndex);

                            WriteList(w, list3);   //write the compound list

                            var list4 = rel.GetParents2Items(itemIndex);

                            WriteList(w, list4);  //write the compound list
                            break;
                    }
                }
            }
        }
        #endregion

        #region ReadData  =====================================================
        public void ReadData(DataReader r, Item[] items)
        {
            var N = r.ReadInt32();      //number of  serialized relations 
            var fv = r.ReadByte();      //format version

            for (int i = 0; i < N; i++)
            {
                if (fv == 1)
                {
                    var irel = r.ReadInt32();    //read relation index
                    if (irel < 0 || irel > items.Length)
                        throw new Exception($"LinkSerializer ReadData, invalid relation index: {irel}");

                    if (!(items[irel] is Relation rel))
                        throw new Exception($"LinkSerializer ReadData, null relation for index: {irel}");

                    var pairing = (Pairing)r.ReadByte();  //read pairing type
                    if (rel.Pairing != pairing)
                        throw new Exception($"LinkSerializer ReadData, invalid relation pairing type");

                    switch (pairing)
                    {
                        case Pairing.OneToOne:
                            {
                                var count1 = r.ReadInt32(); //number of parent/child link pairs in this OntToOne relation

                                var list1 = new (int, int)[count1];
                                var list2 = new (int, int)[count1];

                                for (int j = 0; j < count1; j++)
                                {
                                    var ix1 = r.ReadInt32();
                                    var ix2 = r.ReadInt32();

                                    list1[j] = (ix1, ix2);
                                    list2[j] = (ix2, ix1);
                                }
                                rel.SetChildren1(list1, items);
                                rel.SetParents1(list2, items);
                            }
                            break;

                        case Pairing.OneToMany:
                            {
                                var (len, list1) = ReadList(r);
                                var list2 = new List<(int, int)>(len);
                                foreach (var (ix1, ix2List) in list1)
                                {
                                    foreach (var ix2 in ix2List) { list2.Add((ix2, ix1)); }
                                }
                                rel.SetChildren2(list1, items);
                                rel.SetParents1(list2.ToArray(), items);
                            }
                            break;

                        case Pairing.ManyToMany:
                            {
                                var (_, list1) = ReadList(r);
                                var (_, list2) = ReadList(r);

                                rel.SetChildren2(list1, items);
                                rel.SetParents2(list2, items);
                            }
                            break;
                        default:
                            throw new Exception("LinkSerializer ReadData, invalid pairing");
                    }
                }
                else
                    throw new Exception("LinkSerializer ReadData, invalid format version");
            }
        }
        #endregion

        #region WriteList - write the compond list  ===========================
        void WriteList(DataWriter w, (int, int[])[] list)
        {
            var len = list.Length;
            if (len < 256)
            {
                w.WriteByte(1); //type code specifing there are fewer than 256 items
                w.WriteByte((byte)len);
            }
            else if (len < 65536)
            {
                w.WriteByte(2); //type code specifing there are fewer than 65536 items
                w.WriteUInt16((ushort)len);
            }
            else
            {
                w.WriteByte(4); //type code specifing there are more than 65555 items
                w.WriteInt32(len);
            }
            foreach (var ent in list)
            {
                WriteOneToMany(w, ent);
            }
        }
        #endregion

        #region ReadList - read the compond list  =============================
        (int, (int, int[])[]) ReadList(DataReader r)
        {
            var len = 0;
            var typeCode = r.ReadByte(); //type code for the size of the compound list

            switch (typeCode)
            {
                case 1:
                    len = r.ReadByte();
                    break;
                case 2:
                    len  = r.ReadUInt16();
                    break;
                case 4:
                    len = r.ReadInt32();
                    break;
                 default:
                    throw new Exception($"LinkSerializer ReadData ReadList, invalid list sizing code type");
            }

            var list = new (int, int[])[len];
            for (int i = 0; i < len; i++) { list[i] = ReadOneToMany(r); }

            return (len, list);
        }
        #endregion

        #region WriteOneToMany  ===============================================
        void WriteOneToMany(DataWriter w, (int, int[]) pcList)
        {
            var (ix1, ix2List) = pcList;
            w.WriteInt32(ix1);

            var len = ix2List.Length;            
            if (len < 256)
            {
                w.WriteByte(1); //type code specifing there are fewer than 256 sublist items
                w.WriteByte((byte)len);
            }
            else if (len < 65536)
            {
                w.WriteByte(2); //type code specifing there are fewer than 65526 sublist items
                w.WriteUInt16((ushort)len);
            }
            else
            {
                w.WriteByte(4); //type code specifing there are more than 65525 sublist items
                w.WriteInt32(len);
            }
            foreach (var ix2 in ix2List) { w.WriteInt32(ix2); }
        }
        #endregion

        #region ReadOneToMany  ================================================
        (int, int[]) ReadOneToMany(DataReader r)
        {
            var ix1 = r.ReadInt32();
            var typeCode = r.ReadByte();
            switch (typeCode)
            {
                case 1:
                    var len1 = r.ReadByte();
                    var lst1 = new int[len1];
                    for (int i = 0; i < len1; i++) { lst1[i] = r.ReadInt32(); }
                    return (ix1, lst1);
                case 2:
                    var len2 = r.ReadUInt16();
                    var lst2 = new int[len2];
                    for (int i = 0; i < len2; i++) { lst2[i] = r.ReadInt32(); }
                    return (ix1, lst2);
                case 4:
                    var len4 = r.ReadUInt32();
                    var lst4 = new int[len4];
                    for (int i = 0; i < len4; i++) { lst4[i] = r.ReadInt32(); }
                    return (ix1, lst4);
                default:
                    throw new Exception($"LinkSerializer ReadOneToMany, invalid Ix2List type code");
            }
        }
        #endregion

    }
}
