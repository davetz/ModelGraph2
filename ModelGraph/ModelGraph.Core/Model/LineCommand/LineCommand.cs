using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Shapes;

namespace ModelGraph.Core
{

    public abstract class LineCommand : Item
    {
        internal Action Action { get; }

        internal LineCommand(LineModel model, Action action)
        {
            Owner = model;
            Action = action;
        }
        public void Execute() 
        {
            if (IsValid(Owner))
                DataChef.PostCommand(this);
        }

        public virtual bool IsRemoveCommand => false;
        public virtual bool IsInsertCommand => false;
    }
}
