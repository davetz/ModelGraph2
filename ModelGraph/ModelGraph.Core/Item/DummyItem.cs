﻿
namespace ModelGraph.Core
{
    public class DummyItem : Item
    {
        internal DummyItem(Chef owner)
        {
            Owner = owner;
            OldIdKey = IdKey.DummyItem;
        }
    }
}
