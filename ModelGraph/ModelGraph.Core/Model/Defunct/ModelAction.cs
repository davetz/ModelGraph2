﻿using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    internal class ModelAction
    {/*
        Each LineModel contains an instance of ModelAction.
        Once initialized it constrains and directs the actions 
        initiated from the UI controls. 
     */
        internal Func<ItemModel,ItemModel, bool> ModelUsed;

        internal Func<ItemModel, string> ModelInfo;
        internal Func<ItemModel, string> ModelSummary;
        internal Func<ItemModel, string> ModelDescription;

        internal Func<ItemModel, int> IndexValue;
        internal Func<ItemModel, bool> BoolValue;
        internal Func<ItemModel, string> TextValue;
        internal Func<ItemModel, string[]> ListValue;

        internal Func<ItemModel, List<ItemModel>, (bool, bool)> Validate;

        internal Action<ItemModel, List<ModelCommand>> MenuCommands;
        internal Action<ItemModel, List<ModelCommand>> ButtonCommands;

        internal Func<ItemModel, ItemModel, bool, DropAction> ModelDrop;
        internal Func<ItemModel, ItemModel, bool, DropAction> ReorderItems;

        internal Func<ItemModel, (string Kind, string Name)> ModelKindName;
        internal Func<ItemModel, (string Kind, string Name, int Count, ModelType Type)> ModelParms;
    }
}
