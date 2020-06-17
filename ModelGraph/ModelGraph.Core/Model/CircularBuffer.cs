using System.Collections.Generic;

namespace ModelGraph.Core
{
    /// <summary>Single use circular buffer</summary>
    internal class CircularBuffer<T>
    {
        private T _targetItem;
        private readonly T[] _buffer;
        internal int _overshoot;
        private bool _checkItem;

        internal int Size { get; }
        internal int Count { get; private set; }
        internal int EndCount { get; set; }
        internal bool HitItem { get; private set; }
        internal bool HasRolledOver => Count > Size;


        #region Constructor  ==================================================
        internal CircularBuffer(int size, int endCount)
        {
            _buffer = new T[size];
            Size = size;

            SetEndCount(endCount);
        }

        internal CircularBuffer(int size, T targetItem) //= report finding the target
        {
            _buffer = new T[size];
            Size = size;

            SetTargetItem(targetItem);
        }
        #endregion

        internal void SetEndCount(int endCount) { Count = 0; EndCount = endCount; _checkItem = false; }
        internal void SetEndCount(int count, int endCount) { Count = count; EndCount = endCount; _checkItem = false; }
        internal void SetTargetItem(T item, int overshoot = -1) { Count = 0; _targetItem = item; _overshoot = (overshoot < 0) ? Size / 2 : overshoot; _checkItem = true; HitItem = false; }

        /// <summary>Add item to  buffer, return true: if hit target; false: if hit the end of list</summary>
        internal bool Add(T item)
        {
            var index = Count++ % Size;
            _buffer[index] = item;

            if (HitItem)
            {
                if (_overshoot-- < 0) return true;
            }
            else if (_checkItem)
            {
                if (_targetItem.Equals(item))
                {
                    HitItem = true;
                    _checkItem = false;
                    _overshoot = Size / 2;
                }
            }
            else if (Count >= EndCount) return true;

            return false;
        }

        internal List<T> GetList()
        {
            var list = new List<T>(Size);
            if (Count < Size)
            {
                for (int i = 0; i < Count; i++)
                {
                    list.Add(_buffer[i]);
                }
            }
            else
            {
                for (int index = Count; index < Count + Size; index++)
                {
                    list.Add(_buffer[index % Size]);
                }
            }
            return list;
        }
    }
}
