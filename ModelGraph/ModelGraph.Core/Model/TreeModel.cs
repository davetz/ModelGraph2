using System;
using System.Collections.Generic;
using Windows.Media.Devices.Core;

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
            Depth = 254;
            ControlType = ControlType.PrimaryTree;
            chef.Add(this);

            Add(new X612_DataChefModel(this, chef));
            RefreshViewList(null, ChangeType.NoChange);
        }
        internal TreeModel(RootTreeModel rootModel, Chef chef, IdKey childId) //======== created by the TreeRootModel
        {
            Item = chef;
            Owner = rootModel;
            Depth = 255;
            ControlType = ControlType.PartialTree;

            chef.Add(this);
            switch (childId)
            {
                case IdKey.MetadataRootModel:
                    new X623_MetaRootModel(this, chef);
                    break;
                case IdKey.ModelingRootModel:
                    new X623_MetaRootModel(this, chef);
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
        private bool _modelTreeChanged;
        /// <summary>We are scrolling back and forth in the flattened model hierarchy</summary>
        public (List<LineModel>, LineModel) GetCurrentView(int viewSize, LineModel selectModel, LineModel topModel, LineModel endModel, int scroll)
        {
            bool invalidTopModel = (topModel is null) || topModel.IsInvalid;
            bool invalidEndModel = (endModel is null) || endModel.IsInvalid;
            bool invalidSelectModel = (selectModel is null) || selectModel.IsInvalid;

            var size = viewSize * 3;
            if (_modelTreeChanged || _buffer is null || size > _bufferSize)
            {
                _modelTreeChanged = false;

                _bufferSize = size;
                if (invalidTopModel)
                    _buffer = new CircularBuffer<LineModel>(size, size);
                else
                    _buffer = new CircularBuffer<LineModel>(size, topModel);

                _bufferIsNotAtEndOfList = ModelTreeRoot.BufferFillingTraversal(_buffer);
            }

            var list = _buffer.GetList();
            if (list.Count < viewSize)
                return (list, selectModel);

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
        public void RefreshViewList(LineModel select, ChangeType change = ChangeType.NoChange)
        {
            var invalidSelect = select is null || select.IsInvalid;

            if (!invalidSelect)
            {
                switch (change)
                {
                    case ChangeType.NoChange:
                        break;
                    case ChangeType.GoToEnd:
                        break;
                    case ChangeType.GoToHome:
                        break;
                    case ChangeType.ToggleLeft:
                        _modelTreeChanged |= select.ToggleLeft();
                        break;
                    case ChangeType.ToggleRight:
                        _modelTreeChanged |= select.ToggleRight();
                        break;
                    case ChangeType.ToggleFilter:
                        break;
                    case ChangeType.FilterSortChanged:
                        break;
                }
            }
            var prev = new Dictionary<Item, LineModel>();
            _modelTreeChanged |= ModelTreeRoot.Validate(prev);

            if (_modelTreeChanged) PageControl?.Refresh();
        }
        #endregion

        #region OverrideMethods  ==============================================
        public override (string kind, string name, int count) GetLineParms(Chef chef) => (null, BlankName, 0);
        #endregion
    }
}
