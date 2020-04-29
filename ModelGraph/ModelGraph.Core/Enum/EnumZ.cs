
namespace ModelGraph.Core
{
    public class EnumZ : StoreOf<PairZ>
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
        #endregion

        #region GetEnumIndex  ================================================
        int GetEnumIndex(Chef chef, string name)
        {
            for (int i = 0; i < Count; i++)
            {
                var pz = Items[i];
                if (name == pz.GetSingleNameId(chef)) return i;
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
        string[] GetEnumNames(Chef chef)
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
