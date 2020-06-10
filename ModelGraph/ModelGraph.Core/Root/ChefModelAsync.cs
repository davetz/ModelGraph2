using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelGraph.Core
{
    public partial class Root
    {
        #region PostUIRequest  ================================================
        // These methods are called from the ui thread and typically they invoke 
        // some change to the dataChefs objects (create, remove, update, link, unlink)
        internal void PostAction(IModel model, Action action)
        {
        }
        internal void PostRefresh(IModel model)
        {
        }
        internal void PostCommand(LineCommand command)
        {
            PostModelRequest(command.Action);
        }
        internal void PostSetBoolValue(Item item, Property prop, bool value)
        {
            PostSetStringValue(item, prop, value.ToString());
        }
        internal void PostSetStringValue(Item item, Property prop, string value)
        {
            if (ItemUpdated.IsNotRequired(item, prop, value)) return;

            PostModelRequest(() => { ItemUpdated.Record(this, item, prop, value); });
        }
        internal void PostSetIndexValue(Item item, Property prop, int index)
        {

        }
        #endregion

        #region ExecuteRequest ================================================
        private async void PostModelRequest(Action action)
        {
            await Task.Run(() => { ExecuteRequest(action); }); // runs on worker thread 
            //<=== control immediatley returns to the ui thread
            //(some time later the worker task completes and signals the ui thread)
            //===> the ui thread returns here and continues executing the following code
            foreach (var item in Items)
            {
                if (item is TreeModel tm) tm.Validate();
            }
        }
        private void ExecuteRequest(Action action)
        {
            // the action will likey modify objects, 
            // and we can't have multiple threads stepping on each other
            lock (this)
            {
                action();
            }
        }
        #endregion
    }
}
