using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Shapes;

namespace ModelGraph.Core
{
    /// <summary>Single use circular buffer</summary>
    internal class CircularBuffer<T>
    {
        T[] _buffer;
        private int _len;
        private int _count;
        internal int Count => _count;

        internal CircularBuffer(int size)
        {
            _buffer = new T[size];
            _len = size;
        }

        internal int Add(T item)
        {
            var index = _count++ % _len;
            _buffer[index] = item;
            return _count;
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
