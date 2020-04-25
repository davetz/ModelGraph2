using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{
    internal abstract class LineAction
    {
        virtual internal Func<LineModel, string> Info => null;
        virtual internal Func<LineModel, string> Summary => null;
        virtual internal Func<LineModel, string> Description => null;

        virtual internal Func<LineModel, int> IndexValue => null;
        virtual internal Func<LineModel, bool> BoolValue => null;
        virtual internal Func<LineModel, string> TextValue => null;
        virtual internal Func<LineModel, string[]> ListValue => null;

        virtual internal Func<LineModel, List<ItemModel>, (bool, bool)> Validate => null;

        virtual internal Action<LineModel, List<LineCommand>> MenuCommands => null;
        virtual internal Action<LineModel, List<LineCommand>> ButtonCommands => null;

        virtual internal Func<LineModel, ItemModel, bool, DropAction> ModelDrop => null;
        virtual internal Func<LineModel, ItemModel, bool, DropAction> ReorderItems => null;

        virtual internal Func<LineModel, (string Kind, string Name)> KindName => null;
        virtual internal Func<LineModel, (string Kind, string Name, int Count, ModelType Type)> Parms => null;
    }
}
