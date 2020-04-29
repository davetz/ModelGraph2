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
        internal void PostAction(ItemModelOld model, Action action)
        {
            if (model.IsInvalid) return;

            PostModelRequest(model.GetRootModel(), action);
        }
        internal void PostRefresh(ItemModelOld model)
        {
            PostModelRequest(model.GetRootModel(), () => { });
        }
        internal void PostCommand(ModelCommand command)
        {
            var model = command.Model;
            if (command.Action != null)
                PostModelRequest(model.GetRootModel(), () => { command.Action(model); });
            else if (command.Action1 != null)
                PostModelRequest(model.GetRootModel(), () => { command.Action1(model, command.Parameter1); });
        }
        internal void PostRefreshViewList(RootModelOld root, ItemModelOld select, int scroll, ChangeType change)
        {
            root.SelectModel = select;
            PostModelRequest(root, () => RefreshViewFlatList(root, scroll, change));
        }
        internal void PostSetValue(ItemModelOld model, bool value)
        {
            PostSetValue(model, value.ToString());
        }
        internal void PostSetValue(ItemModelOld model, string value)
        {
            if (model.IsInvalid) return;

            var item = model.Item;
            var prop = model.Aux1 as Property;
            var oldValue = prop.Value.GetString(item);
            if (IsSameValue(value, oldValue)) return;

            PostModelRequest(model.GetRootModel(), () => {SetValue(model, value); });
        }
        internal void PostSetValue(ItemModelOld model, int index)
        {
            if (index < 0) return;
            if (model.IsInvalid) return;


            string[] values;
            if (model.Aux2 is EnumX x)
            {
                values = GetEnumActualValues(x);
                if (index < values.Length) PostSetValue(model, values[index]);
            }
            else if (model.Aux2 is EnumZ z)
            {
                var zvals = GetEnumZNames(z);
                if (index < zvals.Length) PostSetValue(model, zvals[index]);
            }
        }
        #endregion

        #region ExecuteRequest ================================================
        //  Called from the ui thread and runs on a background thred

        private async void PostModelRequest(RootModelOld model, Action action)
        {
            await Task.Run(() => { ExecuteRequest(model, action); }); // runs on worker thread 
            //<=== control immediatley returns to the ui thread

            //(some time later the worker task completes and signals the ui thread)

            //===> the ui thread returns here and continues executing the following code
            var rootModels = _rootModels.ToArray(); // get a copy of the root model list
            foreach (var root in rootModels) { root.PageDispatch(); }
        }
        private void ExecuteRequest(RootModelOld model, Action action)
        {
            // the dataAction will likey modify the dataChef's objects, 
            // so we can't have multiple threads stepping on each other
            lock (this)
            {
                if (model != null && action != null && model.Item != null && model.Item.IsValid) action();

                CheckChanges();

                Refresh(model); //action initiated on model, so refresh it first 
                foreach (var root in _rootModels)
                {
                    if (root != model) Refresh(root); // then do all the others
                }

                void Refresh(RootModelOld m)
                {
                    if (m.HasFlatList) RefreshViewFlatList(m);
                    m.UIRequestRefreshModel();
                }
            }
        }
        #endregion
    }
}
