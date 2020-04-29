
using System;

namespace ModelGraph.Core
{/*

 */
    public class ModelCommand
    {
        public Chef Chef;
        public ItemModelOld Model;
        public Action<ItemModelOld> Action;
        public Action<ItemModelOld, Object> Action1;
        public Object Parameter1;
        public string Name;
        public string Summary;
        internal IdKey IdKey;

        public ModelCommand(Chef chef, ItemModelOld model, IdKey idKe, Action<ItemModelOld> action)
        {
            Chef = chef;
            Model = model;
            IdKey = idKe;
            Action = action;
            Name = chef.GetName(idKe);
            Summary = chef.GetSummary(idKe); ;
        }
        public void Release()
        {
            Chef = null;
            Model = null;
            Action = null;
            Action1 = null;
        }
        public ModelCommand(Chef chef, ItemModelOld model, IdKey idKe, Action<ItemModelOld, Object> action)
        {
            Chef = chef;
            Model = model;
            IdKey = idKe;
            Action1 = action;
            Name = chef.GetName(idKe);
            Summary = chef.GetSummary(idKe); ;
        }

        public void Execute()
        {
            Model.SetIsSelected();
            if (IsInsertCommand) Model.IsExpandedLeft = true;
            Chef.PostCommand(this);
        }

        #region IdKey  ========================================================
        public string AcceleratorKey => Chef.GetAccelerator(IdKey);
        public bool IsStorageFileParameter1 => (IdKey & IdKey.GetStorageFile) != 0;
        public bool IsSaveAsCommand => (IdKey & IdKey.KeyMask) == (IdKey.SaveAsCommand & IdKey.KeyMask);
        public bool IsInsertCommand => (IdKey == IdKey.InsertCommand);
        public bool IsSaveCommand => (IdKey == IdKey.SaveCommand);
        public bool IsCloseCommand => (IdKey == IdKey.CloseCommand);
        public bool IsRemoveCommand => (IdKey == IdKey.RemoveCommand);
        #endregion
    }
}

