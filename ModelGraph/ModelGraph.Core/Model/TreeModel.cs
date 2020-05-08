using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Shapes;

namespace ModelGraph.Core
{
    /// <summary>Flat list of LineModel that emulates a UI tree view</summary>
    public class TreeModel : LineModel, IModel
    {
        public Item RootItem => Item;
        public IPageControl PageControl { get; set; } // reference the UI PageControl       
        public ControlType ControlType { get; private set; }

        public string TitleName => DataChef.TitleName;
        public string TitleSummary => DataChef.TitleSummary;

        internal LineModel ModelTreeRoot => Items[0];
        internal Dictionary<LineModel, string> LineModel_FilterSort = new Dictionary<LineModel, string>();

        #region Constructor  ==================================================
        internal TreeModel(Chef chef) //==================================== invoked in the RootTreeModel constructor
        {
            Owner = Item = chef;
            ControlType = ControlType.PrimaryTree;

            chef.Add(this);

            Add(new X612_DataChefModel(this, chef));
            RefreshViewList(null, ChangeType.NoChange);
        }
        internal TreeModel(RootTreeModel rootModel, Chef chef, IdKey childId) //======== created by the TreeRootModel
        {
            Owner = rootModel;
            Item = chef;
            ControlType = ControlType.PartialTree;

            chef.Add(this);
            switch (childId)
            {
                case IdKey.MetadataRootModel:
                    new X623_MetadataRootModel(this, chef);
                    break;
                case IdKey.ModelingRootModel:
                    new X623_MetadataRootModel(this, chef);
                    break;
                case IdKey.ChangeRootModel:
                    new X622_ChangeRootModel(this, chef);
                    break;
                case IdKey.ErrorRootModel:
                    new X621_ErrorRootModel(this, chef);
                    break;
                default:
                    throw new ArgumentException($"TreeModel constructor, Invalid IdKey child: {childId}");
            }
        }
        #endregion

        #region IModel  =======================================================
        public void Release()
        {
            if (Owner is null) return;

            DataChef.Remove(this);
            Discard(); //discard myself and recursivly discard all my children

            if (this is RootTreeModel)
                DataChef.Discard(); //kill off the dataChef

            Owner = null;
        }
        #endregion

        #region GetCurrentView  ===============================================
        private int _bufferSize;
        private CircularBuffer<LineModel> _buffer;
        private bool _bufferIsNotAtEndOfList;
        /// <summary>We are scrolling back and forth in the flattened model hierarchy</summary>
        public (List<LineModel>, LineModel) GetCurrentView(int viewSize, LineModel selectModel, LineModel topModel, LineModel endModel, int scroll)
        {
            bool invalidTopModel = (topModel is null) || topModel.IsInvalid;
            bool invalidEndModel = (endModel is null) || endModel.IsInvalid;
            bool invalidSelectModel = (selectModel is null) || selectModel.IsInvalid;

            var size = viewSize * 3;
            if (_buffer is null || size > _bufferSize)
            {
                _bufferSize = size;
                if (invalidTopModel)
                    _buffer = new CircularBuffer<LineModel>(size, size);
                else
                    _buffer = new CircularBuffer<LineModel>(size, topModel);

                _bufferIsNotAtEndOfList = ModelTreeRoot.BufferFillingTraversal(_buffer);
            }

            var list = _buffer.GetList();

            if (list.Count == 0) 
                return (list, null);

            if (invalidTopModel || invalidEndModel) 
                return (list.Count < viewSize) ? (list, list[0]) : (list.GetRange(0, viewSize), list[0]);

            var index = list.IndexOf(topModel);
            if (index < 0)
                return (list.Count < viewSize) ? (list, list[0]) : (list.GetRange(0, viewSize), list[0]);

            index += scroll;

            if (index < 0)
            {
                var top = list[0];
                _buffer.SetTargetItem(top);
                _bufferIsNotAtEndOfList = ModelTreeRoot.BufferFillingTraversal(_buffer);

                list = _buffer.GetList();
                index = list.IndexOf(topModel) + scroll;
                if (index < 0)
                    return (list.Count < viewSize) ? (list, list[0]) : (list.GetRange(0, viewSize), list[0]);

                var temp1 = list.GetRange(index, viewSize);
                if (!invalidSelectModel && temp1.Contains(selectModel))
                    return (temp1, selectModel);
                return (temp1, temp1[0]);
            }
            else if (index > list.Count && _bufferIsNotAtEndOfList)
            {
                var end = list[list.Count - 1];
                _buffer.SetTargetItem(end);
                _bufferIsNotAtEndOfList = ModelTreeRoot.BufferFillingTraversal(_buffer);

                list = _buffer.GetList();
                var temp2 = list.GetRange(index, viewSize);
                if (!invalidSelectModel && temp2.Contains(selectModel))
                    return (temp2, selectModel);
                return (temp2, temp2[0]);
            }

            index = list.IndexOf(topModel) + scroll;
            if (index < 0)
                return (list.Count < viewSize) ? (list, list[0]) : (list.GetRange(0, viewSize), list[0]);

            var temp3 = list.GetRange(index, viewSize);
            if (!invalidSelectModel && temp3.Contains(selectModel))
                return (temp3, selectModel);
            return (temp3, temp3[0]);
        }
        #endregion

        #region RefreshViewList  ==============================================
        // Runs on a background thread invoked by the ModelTreeControl 
        private int _fullyFlattenedSize; // flattened tree hierachy length (from the last full traversal)
        private LineModel _selectModel; //the UI is now or will be forced to be focused on this model

        public void RefreshViewList(LineModel select, ChangeType change = ChangeType.NoChange)
        {
            //    if (capacity > 0)
            //    {
            //        var first = IModel.FirstValidModel(viewList);
            //        var start = (first == null);
            //        var previous = new List<IModel>();
            //        var modelStack = new TreeModelStack();

            //        UpdateSelectModel(select, change);

            //        if (root.IsForcedRefresh)
            //        {
            //            modelStack.PushRoot(root);
            //        }
            //        else
            //        {
            //            if (root.ChildModelCount == 0)
            //            {
            //                root.Validate(previous);
            //                root.ViewModels = root.ChildModels;
            //            }
            //            modelStack.PushChildren(root);
            //        }

            //        var S = (scroll < 0) ? -scroll : scroll;
            //        var N = capacity;
            //        var buffer = new CircularBuffer(N, S);

            //        #region GoTo<End,Home>  =======================================
            //        if ((change == ChangeType.GoToEnd || change == ChangeType.GoToHome) && offset >= 0 && first != null)
            //        {
            //            var pm = select.ParentModel;
            //            var ix = pm.GetChildlIndex(select);
            //            var last = pm.ChildModelCount - 1;

            //            if (change == ChangeType.GoToEnd)
            //            {
            //                if (ix < last)
            //                {
            //                    select = pm.ViewModels[last];
            //                    if (!viewList.Contains(select)) FindFirst();
            //                }
            //            }
            //            else
            //            {
            //                if (ix > 0)
            //                {
            //                    select = pm.ViewModels[0];
            //                    if (!viewList.Contains(select)) FindFirst();
            //                }
            //            }
            //            root.SelectModel = select;

            //            void FindFirst()
            //            {
            //                first = select;
            //                var absoluteFirst = root.ViewModels[0];

            //                for (; offset > 0; offset--)
            //                {
            //                    if (first == absoluteFirst) break;

            //                    var p = first.ParentModel;
            //                    var i = p.GetChildlIndex(first);

            //                    first = (i > 0) ? p.ViewModels[i - 1] : p;
            //                }
            //            }

        }
        #endregion

        #region RequieredMethods  =============================================
        internal override (bool anyChange, int flatCount) Validate() => (false, 0);
        public override (string kind, string name, int count) GetLineParms(Chef chef) => (null, BlankName, 0);
        #endregion
    }
}
