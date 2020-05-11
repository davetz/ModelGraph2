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


        #region ValidateBuffer  ===============================================
        private CircularBuffer<LineModel> _buffer;
        /// <summary>Ensure buffer is not null and large enough, return false if is a new buffer</summary>
        private bool ValidateBuffer(int viewSize)
        {
            var size = viewSize * 3;
            if (_buffer is null || size > _buffer.Size)
            {
                _buffer = new CircularBuffer<LineModel>(size, size);
                return false;
            }
            return _buffer.Count > 0;
        }
        #endregion

        #region GetCurrentView  ===============================================
        private int GetScaledSize(int viewSize) => viewSize * 3;
        /// <summary>We are scrolling back and forth in the flattened model hierarchy</summary>
        public (List<LineModel>, LineModel) GetCurrentView(int viewSize, LineModel selected)
        {
            if (!ValidateBuffer(viewSize))
                ModelTreeRoot.BufferFillingTraversal(_buffer);

            var list = _buffer.GetList();
            if (list.Count == 0)
                return (list, null);

            if (list.Count > viewSize)
                list = list.GetRange(0, viewSize);

            if (IsInvalidModel(selected) || !list.Contains(selected))
                selected = list[0];

            return (list, selected);
        }
        #endregion

        #region RefreshViewList  ==============================================
        // Runs on a background thread invoked by the ModelTreeControl 
        public void RefreshViewList(int viewSize, LineModel leading, LineModel selected, ChangeType change = ChangeType.None)
        {
            var chef = DataChef;
            var anyChange = false;
            var isNewBuffer = ValidateBuffer(viewSize);
            bool isValidLead = IsValidModel(leading);
            bool isValidSelect = IsValidModel(selected);

            if (isValidSelect)
            {
                switch (change)
                {
                    case ChangeType.OneDown:
                        if (isValidLead) _buffer.SetTargetItem(leading);
                        ModelTreeRoot.BufferFillingTraversal(_buffer);
                        break;
                    case ChangeType.ToggleLeft:
                        anyChange |= selected.ToggleLeft();
                        if (isValidLead) _buffer.SetTargetItem(leading);
                        ModelTreeRoot.BufferFillingTraversal(_buffer);
                        break;
                    case ChangeType.ToggleRight:
                        anyChange |= selected.ToggleRight();
                        if (isValidLead) _buffer.SetTargetItem(leading);
                        ModelTreeRoot.BufferFillingTraversal(_buffer);
                        break;
                    case ChangeType.ToggleFilter:
                        break;
                    case ChangeType.FilterSortChanged:
                        break;
                }
            }

            if (ChildDelta != chef.ChildDelta)
            {
                ChildDelta = chef.ChildDelta;

                var prev = new Dictionary<Item, LineModel>();
                anyChange |= ModelTreeRoot.Validate(prev);
            }
            if (anyChange) PageControl?.Refresh();
        }
        #endregion

        #region OverrideMethods  ==============================================
        public override (string kind, string name, int count) GetLineParms(Chef chef) => (null, BlankName, 0);
        #endregion
    }
}
