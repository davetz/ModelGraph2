using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    internal class ModelAction
    {/*
        Each LineModel contains an instance of ModelAction.
        Once initialized it constrains and directs the actions 
        initiated from the UI controls. 
     */
        internal Func<ItemModelOld,ItemModelOld, bool> ModelUsed;

        internal Func<ItemModelOld, string> ModelInfo;
        internal Func<ItemModelOld, string> ModelSummary;
        internal Func<ItemModelOld, string> ModelDescription;

        internal Func<ItemModelOld, int> IndexValue;
        internal Func<ItemModelOld, bool> BoolValue;
        internal Func<ItemModelOld, string> TextValue;
        internal Func<ItemModelOld, string[]> ListValue;

        internal Func<ItemModelOld, List<ItemModelOld>, (bool, bool)> Validate;

        internal Action<ItemModelOld, List<ModelCommand>> MenuCommands;
        internal Action<ItemModelOld, List<ModelCommand>> ButtonCommands;

        internal Func<ItemModelOld, ItemModelOld, bool, DropAction> ModelDrop;
        internal Func<ItemModelOld, ItemModelOld, bool, DropAction> ReorderItems;

        internal Func<ItemModelOld, (string Kind, string Name)> ModelKindName;
        internal Func<ItemModelOld, (string Kind, string Name, int Count, ModelType Type)> ModelParms;
    }
}
