using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{

    public class LineCommand : Item
    {
        public Chef Chef { get; internal set; }
        public LineModel Model { get; internal set; }

        public virtual void Execute() { }
        public virtual bool IsRemoveCommand => false;
        public virtual bool IsInsertCommand => false;

    }
}
