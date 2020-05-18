﻿
namespace ModelGraph.Core
{
    public abstract class EnumZ : StoreOf<PairZ>
    {
        internal EnumZ() { }

        #region GetEnumKey  ==================================================
        internal int GetKey(Chef chef, string name)
        {
            foreach (var pz in Items)
            {
                if (name == pz.GetSingleNameId(chef)) return pz.EnumKey;
            }
            return 0;
        }
        internal int GetActualValueAt(int index) => (index < 0 || index >= Count) ? 0 : Items[index].EnumKey;
        #endregion

        #region GetEnumIndex  ================================================
        internal int GetEnumIndex(Chef chef, string name)
        {
            for (int i = 0; i < Count; i++)
            {
                var pz = Items[i];
                if (name == pz.GetSingleNameId(chef)) return i;
            }
            return 0;
        }
        internal int GetEnumIndex(int key)
        {
            for (int i = 0; i < Count; i++)
            {
                var pz = Items[i];
                if (key == (int)(pz.IdKey & IdKey.EnumMask)) return i;
            }
            return 0;
        }
        #endregion

        #region GetEnumName  =================================================
        internal string GetEnumName(Chef chef, int key)
        {
            foreach (var pz in Items)
            {
                if (pz.EnumKey == key) return pz.GetSingleNameId(chef);
            }
            return InvalidItem;
        }
        #endregion

        #region GetEnumNames  ================================================
        internal string[] GetEnumNames(Chef chef)
        {
            var s = new string[Count];

            for (int i = 0; i < Count; i++)
            {
                s[i] = Items[i].GetSingleNameId(chef);
            }
            return s;
        }
        #endregion
    }
}
