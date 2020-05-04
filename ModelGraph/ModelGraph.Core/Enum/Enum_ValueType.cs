
namespace ModelGraph.Core
{/*
 */
    public class Enum_ValueType : EnumZ
    {
        internal override IdKey IdKey => IdKey.ValueTypeEnum;

        #region Constructor  ==================================================
        internal Enum_ValueType(StoreOf<EnumZ> owner)
        {
            Owner = owner;

            CreateChildren();

            owner.Add(this);
        }
        #endregion

        #region CreateChildren  ===============================================
        void CreateChildren()
        {
            Add(new PairZ(this, IdKey.ValueType_Bool));
            Add(new PairZ(this, IdKey.ValueType_Char));
            Add(new PairZ(this, IdKey.ValueType_SByte));
            Add(new PairZ(this, IdKey.ValueType_Int16));
            Add(new PairZ(this, IdKey.ValueType_Int32));
            Add(new PairZ(this, IdKey.ValueType_Int64));
            Add(new PairZ(this, IdKey.ValueType_Single));
            Add(new PairZ(this, IdKey.ValueType_Double));
            Add(new PairZ(this, IdKey.ValueType_Decimal));
            Add(new PairZ(this, IdKey.ValueType_DateTime));
            Add(new PairZ(this, IdKey.ValueType_String));
            Add(new PairZ(this, IdKey.ValueType_Byte));
            Add(new PairZ(this, IdKey.ValueType_UInt16));
            Add(new PairZ(this, IdKey.ValueType_UInt32));
            Add(new PairZ(this, IdKey.ValueType_UInt64));
            Add(new PairZ(this, IdKey.ValueType_BoolArray));
            Add(new PairZ(this, IdKey.ValueType_CharArray));
            Add(new PairZ(this, IdKey.ValueType_SByteArray));
            Add(new PairZ(this, IdKey.ValueType_Int16Array));
            Add(new PairZ(this, IdKey.ValueType_Int32Array));
            Add(new PairZ(this, IdKey.ValueType_Int64Array));
            Add(new PairZ(this, IdKey.ValueType_SingleArray));
            Add(new PairZ(this, IdKey.ValueType_DoubleArray));
            Add(new PairZ(this, IdKey.ValueType_DecimalArray));
            Add(new PairZ(this, IdKey.ValueType_StringArray));
            Add(new PairZ(this, IdKey.ValueType_ByteArray));
            Add(new PairZ(this, IdKey.ValueType_UInt16Array));
            Add(new PairZ(this, IdKey.ValueType_UInt32Array));
            Add(new PairZ(this, IdKey.ValueType_UInt64Array));
        }
        #endregion
    }
}
