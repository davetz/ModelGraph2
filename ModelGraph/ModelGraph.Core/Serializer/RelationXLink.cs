using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    class RelationXLink : ISerializer
    {
        static Guid _serializerGuid => new Guid("61662F08-F43A-44D9-A9BB-9B0126492B8C");
        static byte _formatVersion = 1;
        readonly RelationXStore _rxStore;

        internal RelationXLink(Chef chef, RelationXStore rxStore)
        {
            _rxStore = rxStore;
            chef.RegisterSerializer((_serializerGuid, this), true);
        }

        #region ISerializer  ==================================================
        public bool HasData()
        {
            foreach (var rx in _rxStore.Items)
            {
                if (rx.HasLinks) return true;
            }
            return false;
        }
        public void WriteData(DataWriter w, Dictionary<Item, int> itemIndex)
        {
            var N = 0;
            foreach (var rx in _rxStore.Items) { if (rx.HasLinks) N++; } //count number of serialized relations 

            w.WriteGuid(_serializerGuid);   //serializer guid
            w.WriteInt32(N);                //number of serialized relations 
            w.WriteByte(_formatVersion);    //format version

            foreach (var rx in _rxStore.Items)  //foreach relation entry
            {
                if (rx.HasLinks)
                {
                    w.WriteInt32(itemIndex[rx]);    //relation index
                    w.WriteByte((byte)rx.Pairing);  //pairing cross check, it should match the relation.pairing on reading

                    switch (rx.Pairing)
                    {
                        case Pairing.OneToOne:

                            var list1 = rx.GetChildren1Items(itemIndex);

                            w.WriteInt32(list1.Length);   //number of parent/child link pairs in this OntToOne relation
                            foreach (var (ix1, ix2) in list1)
                            {
                                w.WriteInt32(ix1);      //parent item
                                w.WriteInt32(ix2);      //child item
                            }
                            break;

                        case Pairing.OneToMany:

                            var list2 = rx.GetChildren2Items(itemIndex);

                            WriteList(list2);   //write the  parent/childList link pairs
                            break;

                        case Pairing.ManyToMany:

                            var list3 = rx.GetChildren2Items(itemIndex);

                            WriteList(list3);   //write the  parent/childList link pairs

                            var list4 = rx.GetParents2Items(itemIndex);

                            WriteList(list4);  //write the  child/parentList link pairs
                            break;
                    }
                }
            }
            #region WriteList - (write the compond sublist)  ==================
            void WriteList((int, int[])[] list)
            {
                w.WriteInt32(list.Length);   //number of items in main list

                var max = 0;
                foreach (var (_, ls) in list)
                {
                    var n = ls.Length;
                    if (n > max) max = n;
                }

                if (max < 256)
                {
                    w.WriteByte(1); //type code for the size of the max count of items in sublist

                    foreach (var (ix1, ix2List) in list)
                    {
                        w.WriteInt32(ix1);  //item index1

                        w.WriteByte((byte)ix2List.Length); // number sublist items
                        foreach (var ix2 in ix2List)
                        {
                            w.WriteInt32(ix2);  // sublist item index2
                        }
                    }
                }
                if (max < 65536)
                {
                    w.WriteByte(2); //type code for the size of the max count of items in sublist

                    foreach (var (ix1, ix2Lst) in list)
                    {
                        w.WriteInt32(ix1);  // item index1

                        w.WriteUInt16((ushort)ix2Lst.Length); // number sublist items
                        foreach (var ix2 in ix2Lst)
                        {
                            w.WriteInt32(ix2);  //item index1
                        }
                    }
                }
                else
                {
                    w.WriteByte(4); //type code for the size of the max count of items in sublist

                    foreach (var (ix1, ix2Lst) in list)
                    {
                        w.WriteInt32(ix1);  // item index1

                        w.WriteInt32((ushort)ix2Lst.Length); // number sublist items
                        foreach (var ix2 in ix2Lst)
                        {
                            w.WriteInt32(ix2);  //item index1
                        }
                    }
                }
            }
            #endregion
        }
        public void ReadData(DataReader r, Item[] items)
        {
            var N = r.ReadInt32();      //number of  serialized relations 
            var fv = r.ReadByte();      //format version

            if (fv == 1)
            {
                for (int i = 0; i < N; i++)
                {
                    var irx = r.ReadInt32();    //read relation index
                    if (irx < 0 || irx > items.Length)
                        throw new Exception($"RelationXLink ReadData, invalid relation index: {irx}");

                    var rx = items[irx] as RelationX;
                    if (rx is null)
                        throw new Exception($"RelationXLink ReadData, null relation for index: {irx}");

                    var pairing = (Pairing)r.ReadByte();  //read pairing type
                    if (rx.Pairing != pairing)
                        throw new Exception($"RelationXLink ReadData, invalid relation pairing type");

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
                                rx.SetChildren1(list1, items);
                                rx.SetParents1(list2, items);
                            }
                            break;

                        case Pairing.OneToMany:
                            {
                                var (totalCount, list1) = ReadList();
                                var list2 = new List<(int, int)>(totalCount);
                                foreach (var (ix1, ix2List) in list1)
                                {
                                    foreach (var ix2 in ix2List)
                                    {
                                        list2.Add((ix2, ix1));
                                    }
                                }
                                rx.SetChildren2(list1, items);
                                rx.SetParents1(list2.ToArray(), items);
                            }
                            break;

                        case Pairing.ManyToMany:
                            {
                                var (_, list1) = ReadList();
                                var (_, list2) = ReadList();

                                rx.SetChildren2(list1, items);
                                rx.SetParents2(list2, items);
                            }
                            break;
                    }
                }
            }
            else
                throw new Exception("RelationXLink ReadData, invalid format version");

            #region ReadList - (read the compond sublist)  ====================
            (int, (int, int[])[]) ReadList()
            {
                var count1 = r.ReadInt32(); //number of items in main list
                var mainList = new (int, int[])[count1];
                var totalCount = 0;
                for (int j = 0; j < count1; j++)
                {
                    var code = r.ReadByte(); //type code for the size of the max count of items in sublist
                    switch (code)
                    {
                        case 1:
                            {
                                var ix1 = r.ReadInt32();
                                var count2 = r.ReadByte();   //read number sublist items
                                var ix2List = new int[count2];
                                for (int k = 0; k < count2; k++)
                                {
                                    ix2List[k] = r.ReadInt32();
                                }
                                mainList[j] = (ix1, ix2List);
                                totalCount += count2;
                            }
                            break;

                        case 2:
                            {
                                var ix1 = r.ReadInt32();
                                var count2 = r.ReadUInt16();   //read number sublist items
                                var ix2List = new int[count2];
                                for (int k = 0; k < count2; k++)
                                {
                                    ix2List[k] = r.ReadInt32();
                                }
                                mainList[j] = (ix1, ix2List);
                                totalCount += count2;
                            }
                            break;

                        case 4:
                            {
                                var ix1 = r.ReadInt32();
                                var count2 = r.ReadInt32();   //read number sublist items
                                var ix2List = new int[count2];
                                for (int k = 0; k < count2; k++)
                                {
                                    ix2List[k] = r.ReadInt32();
                                }
                                mainList[j] = (ix1, ix2List);
                                totalCount += count2;
                            }
                            break;

                        default:
                            throw new Exception($"RelationXLink ReadData ReadList, invalid list sizing code type");
                    }
                }
                return (totalCount, mainList);
            }
            #endregion
        }
        #endregion
    }
}
