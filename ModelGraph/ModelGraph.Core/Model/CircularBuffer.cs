using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Shapes;

namespace ModelGraph.Core
{
    /// <summary>Single use circular buffer</summary>
    internal class CircularBuffer<T>
    {
        private T _targetItem;
        private readonly T[] _buffer;
        private readonly int _len;
        private int _delay;
        private bool _checkItem;

        internal int Count { get; private set; }
        internal int EndCount { get; set; }
        internal bool HitItem { get; private set; }
        internal bool HasRolledOver => Count > _len;

        #region Constructor  ==================================================
        internal CircularBuffer(int size, int endCount)
        {
            _buffer = new T[size];
            _len = size;

            SetEndCount(endCount);
        }

        internal CircularBuffer(int size, T targetItem) //= report finding the target
        {
            _buffer = new T[size];
            _len = size;

            SetTargetItem(targetItem);
        }
        #endregion

        internal void SetEndCount(int endCount) { Count = 0; EndCount = endCount; _checkItem = false; }
        internal void SetEndCount(int count, int endCount) { Count = count; EndCount = endCount; _checkItem = false; }
        internal void SetTargetItem(T item) { Count = 0;  _targetItem = item; _checkItem = true; }

        /// <summary>Add item to  buffer, return true: if hit target; false: if hit the end of list</summary>
        internal bool Add(T item)
        {
            var index = Count++ % _len;
            _buffer[index] = item;

            if (HitItem)
            {
                if (_delay-- < 0) return true;
            }
            else if (_checkItem)
            {
                if (_targetItem.Equals(item))
                {
                    HitItem = true;
                    _checkItem = false;
                    _delay = _len / 2;
                }
            }
            else if (Count >= EndCount) return true;

            return false;
        }

        internal List<T> GetList()
        {
            var list = new List<T>(_len);
            if (Count < _len)
            {
                for (int i = 0; i < Count; i++)
                {
                    list.Add(_buffer[i]);
                }
            }
            else
            {
                for (int index = Count; index < Count + _len; index++)
                {
                    list.Add(_buffer[index % _len]);
                }
            }
            return list;
        }
    }
}
