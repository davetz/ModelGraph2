
using System;

namespace ModelGraph.Core
{
    public class X035_InsertCommand : LineCommand
    {
        internal override IdKey IdKey => IdKey.InsertCommand;
        public override bool IsInsertCommand => true;

        internal X035_InsertCommand(LineModel owner, Action action) : base(owner, action) { }
    }
}
