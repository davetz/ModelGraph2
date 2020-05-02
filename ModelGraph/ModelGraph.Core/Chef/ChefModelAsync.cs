using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelGraph.Core
{
    public partial class Chef
    {
        internal IList<IModel> GetChildModels(RootTreeModel root)
        {
            var list = new List<IModel>(10);
            foreach (var item in Items)
            {
                if (item == root) continue;
                if (item is IModel m) list.Add(m);
            }
            return list;
        }

        #region PostRequest  ==================================================
        // These methods are called from the ui thread and typically they invoke 
        // some type of change to the dataChefs objects (create, remove, update)
        internal void PostAction(IModel model, Action action)
        {
        }
        internal void PostRefresh(IModel model)
        {
        }
        internal void PostCommand(LineCommand command)
        {
        }
        internal void PostSetValue(IModel model, bool value)
        {
        }
        internal void PostSetValue(IModel model, string value)
        {
        }
        internal void PostSetValue(IModel model, int index)
        {
        }
        #endregion

        #region ExecuteRequest ================================================
        private async void PostModelRequest(IModel model, Action action)
        {
            await Task.Run(() => { ExecuteRequest(model, action); }); // runs on worker thread 
            //<=== control immediatley returns to the ui thread
            //(some time later the worker task completes and signals the ui thread)
            //===> the ui thread returns here and continues executing the following code
        }
        private void ExecuteRequest(IModel model, Action action)
        {
            // the action will likey modify objects, 
            // and we can't have multiple threads stepping on each other
            lock (this)
            {
            }
        }
        #endregion
    }
}
