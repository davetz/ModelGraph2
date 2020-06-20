using System;
using System.Collections.Generic;
using Windows.Media.Devices.Core;

namespace ModelGraph.Core
{
    /// <summary>Flat list of LineModel that emulates a UI tree view</summary>
    public abstract class TreeModel : LineModel, IModel
    {
        public Item RootItem => Item;
        public IPageControl PageControl { get; set; } // reference the UI PageControl       
        public ControlType ControlType { get; private set; }

        public string TitleName => DataRoot.TitleName;
        public string TitleSummary => DataRoot.TitleSummary;

        internal LineModel ModelTreeRoot => Items[0];

        #region Constructor  ==================================================
        internal TreeModel(Root root) //==================================== invoked in the RootTreeModel constructor
        {
            Owner = Item = root;
            Depth = 254;
            ControlType = ControlType.PrimaryTree;
            root.Add(this);

            Add(new RootModel_612(this, root));
        }
        #endregion

        #region IModel  =======================================================
        public void Release()
        {
            if (Owner is null) return;

            DataRoot.Remove(this);
            Discard(); //discard myself and recursivly discard all my children

            if (this is RootModel)
                DataRoot.Discard(); //kill off the dataChef

            Owner = null;
        }
        #endregion

        #region FilterSort  ===================================================
        private readonly Dictionary<LineModel, FilterSort> _model_FilterSort = new Dictionary<LineModel, FilterSort>();

        public int FilterCount(LineModel model) => _model_FilterSort.TryGetValue(model, out FilterSort filter) ? filter.Count : 0;

        public string ViewFilter(LineModel model) => _model_FilterSort.TryGetValue(model, out FilterSort filter) ? filter.FilterString : string.Empty;

        public void SetFilterText(LineModel model, string filterText)
        {
            if (_model_FilterSort.TryGetValue(model, out FilterSort filter))
            {
                if (filter.SetFilter(model, filterText)) _model_FilterSort.Remove(model);

            }
            else if (!string.IsNullOrWhiteSpace(filterText))
            {
                _model_FilterSort.Add(model, new FilterSort(model, filterText));
            }
            PageControl?.Refresh();
        }
        public virtual void ClearViewFilter(LineModel model) 
        { 
            if (_model_FilterSort.TryGetValue(model, out FilterSort filter)) filter.SetFilter(model, null); 
        }

        public (Sorting, Usage, string) GetFilterSort(LineModel model) => _model_FilterSort.TryGetValue(model, out FilterSort filter) ? filter.Parms : (Sorting.Unsorted, Usage.None, string.Empty);

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
            var root = DataRoot;
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
                        ModelTreeRoot.BufferFillingTraversal(_buffer, _model_FilterSort);
                        break;
                    case ChangeType.ToggleLeft:
                        anyChange |= selected.ToggleLeft();
                        if (isValidLead) _buffer.SetTargetItem(leading);
                        ModelTreeRoot.BufferFillingTraversal(_buffer, _model_FilterSort);
                        break;
                    case ChangeType.ToggleRight:
                        anyChange |= selected.ToggleRight();
                        if (isValidLead) _buffer.SetTargetItem(leading);
                        ModelTreeRoot.BufferFillingTraversal(_buffer, _model_FilterSort);
                        break;
                    case ChangeType.ToggleFilter:
                        selected.IsFilterVisible = !selected.IsFilterVisible;
                        break;
                    case ChangeType.FilterSortChanged:
                        ModelTreeRoot.BufferFillingTraversal(_buffer, _model_FilterSort);
                        break;
                }
            }

            if (anyChange) PageControl?.Refresh();
        }
        #endregion

        #region Validate  =====================================================
        internal void Validate()
        {
            if (ModelTreeRoot.Validate(new Dictionary<Item, LineModel>())) 
                PageControl?.Refresh();
        }
        #endregion

        #region OverrideMethods  ==============================================
        public override (string, string) GetKindNameId(Root root) => (null, BlankName);
        #endregion
    }
}
