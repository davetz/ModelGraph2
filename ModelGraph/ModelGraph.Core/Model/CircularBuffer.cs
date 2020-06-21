using System.Collections.Generic;

namespace ModelGraph.Core
{
    /// <summary>Continous circular buffer</summary>
    internal class CircularBuffer<T>
    {
        private T _targetItem;                      // one of two end point triggers
        private readonly T[] _buffer;               // continous circular buffer
        internal int _overshoot;                    // keep adding items, overshooting the target by half the buffer size
        private bool _checkingForItem;              // expecting and checking for the target item to be added

        internal int Size { get; }                  // N * the number of lines in the UI window, allow for some UI window scrolling with out having to traverse the model hierarchy 
        internal int Count { get; private set; }    // number of items added to the buffer since Initialize
        internal int EndCount { get; set; }         // alternative end point trigger
        internal bool TargetIsInBuffer { get; private set; }    // detected the target being added to the buffer
        internal bool HasRolledOver => Count > Size;            // the circular aspect of the buffer has been realized

        #region Constructor  ==================================================
        internal CircularBuffer(int size)
        {
            _buffer = new T[size];
            Count = 0;
            Size = size;
            EndCount = size;
            _checkingForItem = false;
        }
        #endregion

        #region Initialize  ===================================================
        internal void Initialize(T targetItem, int overshoot = -1) 
        { 
            Count = 0; 
            _targetItem = targetItem; 
            _overshoot = (overshoot < 0) ? Size / 2 : overshoot; 
            _checkingForItem = true; 
            TargetIsInBuffer = false;
        }
        #endregion

        #region AddItem  ======================================================
        /// <summary>Add item to  buffer, return true: if hit target; false: if hit the end of list</summary>
        internal bool AddItem(T item)
        {
            var index = Count++ % Size;
            _buffer[index] = item;

            if (TargetIsInBuffer)
            {
                if (_overshoot-- < 0) return true;      // one of two end point triggers
            }
            else if (_checkingForItem)
            {
                if (_targetItem.Equals(item))
                {
                    TargetIsInBuffer = true;
                    _checkingForItem = false;
                    _overshoot = Size / 2;
                }
            }
            else if (Count >= EndCount) return true;    // alternative end point trigger

            return false;
        }
        #endregion

        #region GetList  ======================================================
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
        #endregion
    }
}
