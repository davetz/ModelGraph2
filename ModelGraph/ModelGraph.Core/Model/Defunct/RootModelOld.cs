using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public class RootModelOld : ItemModelOld
    {
        public Chef Chef { get; private set; }
        public IPageControl PageControl { get; set; } // reference the UI PageControl

        // used by the ModelTreeControl
        public int ViewCapacity = 25; // updated when the view screen size changes
        public ItemModelOld SelectModel;   // the user selected model
        public List<ItemModelOld> ViewFlatList = new List<ItemModelOld>(); // flat list of models visible to the user 
        internal bool HasFlatList => ControlType == ControlType.PrimaryTree || ControlType == ControlType.PartialTree;

        public ControlType ControlType;

        #region UIRequest  ====================================================
        internal void UIRequestSaveModel() { }
        internal void UIRequestCloseModel() { }
        internal void UIRequestReloadModel() { }
        internal void UIRequestSaveAsModel() { }
        internal void UIRequestRefreshModel() { }

        internal void UIRequestCreatePage(ControlType type, IdKey idKe, Item item, ModelAction get) { }


        internal void UIRequestSaveSymbol() { }
        internal void UIRequestReloadSymbol() { }
        #endregion

        #region PageDispatch  =================================================
        internal void PageDispatch()
        {
        }
        #endregion
    }
}
