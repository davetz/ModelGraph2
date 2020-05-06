using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Shapes;

namespace ModelGraph.Core
{
    /// <summary>Single use circular buffer</summary>
    internal class CircularBuffer<T>
    {
        T _target;
        T[] _buffer;
        private int _len;
        private int _delay;
        private bool _haveTarget;
        private bool _checkTarget;
        private int _count;
        internal int Count => _count;
        internal bool HaveTarget => _haveTarget;

        #region Constructor  ==================================================
        internal CircularBuffer(int size)
        {
            _buffer = new T[size];
            _len = size;
        }

        internal CircularBuffer(int size, T target)
        {
            _buffer = new T[size];
            _len = size;

            _target = target;
            _checkTarget = true;
        }
        #endregion

        internal (int, bool) Add(T item)
        {
            var index = _count++ % _len;
            _buffer[index] = item;
            if (_haveTarget)
            {
                if (_delay-- < 0) return (_count, true);
            }
            else if (_checkTarget)
            {
                if (_target.Equals(item))
                {
                    _haveTarget = true;
                    _checkTarget = false;
                    _delay = _len / 2;
                }
            }
            return (_count, false);
        }

        internal List<T> GetList()
        {
            var list = new List<T>(_len);
            if (_count < _len)
            {
                for (int i = 0; i < _count; i++)
                {
                    list.Add(_buffer[i]);
                }
            }
            else
            {
                for (int index = _count; index < _count + _len; index++)
                {
                    list.Add(_buffer[index % _len]);
                }
            }
            return list;
        }
    }
}
