
using System;

namespace ModelGraph.Core
{/*

 */
    public class ModelCommand
    {
        public Chef Chef;
        public ItemModel Model;
        public Action<ItemModel> Action;
        public Action<ItemModel, Object> Action1;
        public Object Parameter1;
        public string Name;
        public string Summary;
        internal IdKey Trait;

        public ModelCommand(Chef chef, ItemModel model, IdKey trait, Action<ItemModel> action)
        {
            Chef = chef;
            Model = model;
            Trait = trait;
            Action = action;
            Name = chef.GetName(trait);
            Summary = chef.GetSummary(trait); ;
        }
        public void Release()
        {
            Chef = null;
            Model = null;
            Action = null;
            Action1 = null;
        }
        public ModelCommand(Chef chef, ItemModel model, IdKey trait, Action<ItemModel, Object> action)
        {
            Chef = chef;
            Model = model;
            Trait = trait;
            Action1 = action;
            Name = chef.GetName(trait);
            Summary = chef.GetSummary(trait); ;
        }

        public void Execute()
        {
            Model.SetIsSelected();
            if (IsInsertCommand) Model.IsExpandedLeft = true;
            Chef.PostCommand(this);
        }

        #region Trait  ========================================================
        public string AcceleratorKey => Chef.GetAccelerator(Trait);
        public bool IsStorageFileParameter1 => (Trait & IdKey.GetStorageFile) != 0;
        public bool IsSaveAsCommand => (Trait & IdKey.KeyMask) == (IdKey.SaveAsCommand & IdKey.KeyMask);
        public bool IsInsertCommand => (Trait == IdKey.InsertCommand);
        public bool IsSaveCommand => (Trait == IdKey.SaveCommand);
        public bool IsCloseCommand => (Trait == IdKey.CloseCommand);
        public bool IsRemoveCommand => (Trait == IdKey.RemoveCommand);
        #endregion
    }
}

