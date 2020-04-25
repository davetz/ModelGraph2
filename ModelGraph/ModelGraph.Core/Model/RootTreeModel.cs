using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Core
{
    public class RootTreeModel : TreeModel, IRootModel
    {
        public IList<IModel> ChildModels => DataChef.GetChildModels(this);

        #region Constructor  ==================================================
        internal RootTreeModel() : base (new Chef())
        {
            RootModel = this;
        }
        #endregion
    }
}
