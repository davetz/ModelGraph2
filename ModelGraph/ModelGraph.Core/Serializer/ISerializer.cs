﻿using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage.Streams;

namespace ModelGraph.Core
{
    public interface ISerializer
    {
        bool HasData();
        void ReadData(DataReader r, Item[] items);
        void WriteData(DataWriter w, Dictionary<Item, int> itemIndex);
    }
}