using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    internal class ValueDictionary<T> : IValueStore<T>
    {
        ComputeX _owner;
        Dictionary<Item, T> _values;
        protected T _default;

        internal ValueDictionary(int capacity, T defaultValue)
        {
            _default = defaultValue;
            if (capacity > 0) _values = new Dictionary<Item, T>(capacity);
        }
        public void Release()
        {
            _owner = null;
            Clear();
            _values = null;
        }

        public int Count => (_values == null) ? 0 : _values.Count;
        public bool IsSpecific(Item key) => (_values == null) ? false :_values.ContainsKey(key);

        public void Clear()
        {
            if (_values != null)
                _values.Clear();
        }
        public void Remove(Item key)
        {
            if (_values != null)
                _values.Remove(key);
        }
        public void SetOwner(ComputeX cx) => _owner = cx;

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =

        internal T DefaultValue => _default;
        internal IList<Item> GetKeys() => (Count == 0) ? null : _values.Keys.ToArray();
        internal IList<T> GetValues() => (Count == 0) ? null : _values.Values.ToArray();

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =

        public bool GetVal(Item key, out T val)
        {
            if (_values != null && _values.TryGetValue(key, out val))
                return true;

            if (_owner == null)
            {
                val = _default;
                return false;
            }

            if (_owner.GetChef().TryGetComputedValue(_owner, key))
                return _values.TryGetValue(key, out val);
            else
            {
                val = _default;
                return false;
            }
        }

        //= = = = = = = = = = = = = = = = = = = = = = = = = = = = = =

        public bool SetVal(Item key, T value)
        {
            if (_values == null) _values = new Dictionary<Item, T>();

            if (_default == null)
            {
                if (value != null)
                    _values[key] = value;
            }
            else
            {
                if (value != null && value.Equals(_default))
                    _values.Remove(key);
                else
                    _values[key] = value;
            }
            return true;
        }

        // LoadValue() should only be used by RepositoryRead
        public void LoadValue(Item key, T value) => _values[key] = value;

        internal static void ReadData(DataReader r, ColumnX cx, Item[] items)
        {
            var t = r.ReadByte();
            if (t > _createValue.Length) throw new ArgumentException("Invalid Value Type");

            var count = r.ReadInt32();
            if (count < 0) throw new Exception($"Invalid row count {count}");
            
            cx.Value = _createValue[t](r, count, items);
        }

        #region _readValueDictionary  =========================================
        static Func<DataReader, int, Item[], Value>[] _createValue =
        {
            (r, c, i) => new BoolValue(r, c, i),           //  0
            (r, c, i) => new BoolArrayValue(r, c, i),      //  1
            (r, c, i) => new CharValue(r, c, i),           //  2
            (r, c, i) => new CharArrayValue(r, c, i),      //  3
            (r, c, i) => new ByteValue(r, c, i),           //  4
            (r, c, i) => new ByteArrayValue(r, c, i),      //  5
            (r, c, i) => new SByteValue(r, c, i),          //  6
            (r, c, i) => new SByteArrayValue(r, c, i),     //  7
            (r, c, i) => new Int16Value(r, c, i),          //  8
            (r, c, i) => new Int16ArrayValue(r, c, i),     //  9
            (r, c, i) => new UInt16Value(r, c, i),         // 10
            (r, c, i) => new UInt16ArrayValue(r, c, i),    // 11
            (r, c, i) => new Int32Value(r, c, i),          // 12
            (r, c, i) => new Int32ArrayValue(r, c, i),     // 13
            (r, c, i) => new UInt32Value(r, c, i),         // 14
            (r, c, i) => new UInt32ArrayValue(r, c, i),    // 15
            (r, c, i) => new Int64Value(r, c, i),          // 16
            (r, c, i) => new Int64ArrayValue(r, c, i),     // 17
            (r, c, i) => new UInt64Value(r, c, i),         // 18
            (r, c, i) => new UInt64ArrayValue(r, c, i),    // 19
            (r, c, i) => new SingleValue(r, c, i),         // 20
            (r, c, i) => new SingleArrayValue(r, c, i),    // 21
            (r, c, i) => new DoubleValue(r, c, i),         // 22
            (r, c, i) => new DoubleArrayValue(r, c, i),    // 23
            (r, c, i) => new DecimalValue(r, c, i),        // 24
            (r, c, i) => new DecimalArrayValue(r, c, i),   // 25
            (r, c, i) => new DateTimeValue(r, c, i),       // 26
            (r, c, i) => new DateTimeArrayValue(r, c, i),  // 27
            (r, c, i) => new StringValue(r, c, i),         // 28
            (r, c, i) => new StringArrayValue(r, c, i),    // 29
        };
        #endregion


    }
}
