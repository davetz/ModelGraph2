﻿using System.Collections.Generic;

namespace ModelGraph.Core
{
    /// <summary>Continous circular buffer</summary>
    internal class CircularBuffer<T>
    {
        private T[] _buffer;                // continous circular buffer
        private T _target;                  // end point trigger
        private int _size;                  // actual size of buffer
        private int _uiSize;                // max posible number of visible UI rows 
        private int _count;                 // number of items added to the buffer since Initialize
        private int _overshoot;             // keep adding items, overshooting the target by half the buffer size
        private bool _checkingTarget;       // expecting and checking for the target item to be added
        private bool _foundTarget;          // detected the target being added to the buffer

        internal bool IsEmpty => _count == 0;

        #region Constructor  ==================================================
        internal CircularBuffer(int uiSize)
        {
            Validate(uiSize);
        }
        #endregion

        #region Initialize/Validate  ==========================================
        internal void Initialize(T target, int uiSize)
        {
            _target = target;
            Validate(uiSize);
        }
        private void Validate(int uiSize)
        {
            if (_uiSize != uiSize)
            {
                _uiSize = uiSize;
                var minSize = 3 * uiSize;
                if (minSize > _size)
                {
                    _size = minSize;
                    _buffer = new T[minSize];
                }
            }
            _count = 0;
            _foundTarget = false;
            _checkingTarget = (_target != null);
        }
        #endregion

        #region AddItem  ======================================================
        /// <summary>Add item to  buffer, return true: if hit target</summary>
        internal bool AddItem(T item)
        {
            var index = _count++ % _size;
            _buffer[index] = item;

            if (_foundTarget)
            {
                return (_overshoot-- < 0);      // end point triggers

            }
            else if (_checkingTarget)
            {
                if (_target.Equals(item))
                {
                    _foundTarget = true;
                    _checkingTarget = false;
                    _overshoot = _uiSize * 2;
                }
                return false;
            }

            return (_count > _size);    // could not find the target, so abort because the buffer is full
        }
        #endregion

        #region SetOffset/GetList  ============================================
        internal bool IsInvalidOffset(int offset)
        {
            var (start, count, uiStart, uiCount) = GetStartCount();
            var index = uiStart + offset;
            if (offset < 0)
            {
                if (index >= start)
                {
                    _target = _buffer[index % _size];
                    return false;
                }
                _target = _buffer[start % _size];
            }
            else if (offset > 0)
            {
                if ((index + _uiSize) < count)
                {
                    _target = _buffer[index % _size];
                    return false;
                }
                _target = _buffer[(count - 1) % _size];
            }
            _count = 0;
            _foundTarget = false;
            _checkingTarget = (_target != null);
            return true;
        }
        internal List<T> GetList()
        {
            var (_, _, uiStart, uiCount) = GetStartCount();
            var list = new List<T>(uiCount);
            for (int i = 0, j = uiStart; i < uiCount; i++, j++)
            {
                list.Add(_buffer[j % _size]);
            }
            return list;
        }
        #endregion

        #region Parms  ========================================================
        private (int start, int count, int uiStart, int uiCount) GetStartCount()
        {
            int start, count, uiStart, uiCount;
            if (_count < _size)
            {
                start = 0;
                count = _count;
            }
            else
            {
                start = (_count - 1) % _size;
                count = _size;
            }
            uiStart = GetUiStart();
            var ending = start + count;
            uiCount = ending - uiStart;
            if (uiCount > _uiSize) uiCount = _uiSize;
            return (start, count, uiStart, uiCount);
        }
        private int GetUiStart()
        {
            if (_target != null)
            {
            for (int i = 0; i < _size; i++)
            {
                if (_buffer[i].Equals(_target)) return i;
            }
            }
            return 0;
        }
        #endregion
    }
}
