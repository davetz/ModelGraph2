using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    /// <summary>Flat list of LineModel that emulates a UI tree view</summary>
    public abstract class TreeModel : LineModel, IRootModel
    {
        private CircularBuffer<LineModel> _buffer = new CircularBuffer<LineModel>(20);

        public Item RootItem => Item;
        public IPageControl PageControl { get; set; } // reference the UI PageControl       
        public ControlType ControlType { get; private set; }

        #region Constructor  ==================================================
        internal TreeModel(Root root) //========== invoked in the RootModel constructor
        {
            Owner = Item = root;
            Depth = 254;
            ControlType = ControlType.PrimaryTree;

            new RootModel_612(this, root);
            root.Add(this);
        }
        internal TreeModel(Root root, Action<Root,TreeModel> newLineModel)
        {
            Owner = Item = root;
            Depth = 254;
            ControlType = ControlType.PartialTree;

            newLineModel(root, this);
            root.Add(this);
        }
        #endregion

        #region IRootModel  ===================================================
        public string TitleName => DataRoot.TitleName;
        public string TitleSummary => DataRoot.TitleSummary;

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

        #region FilterParms  ==================================================
        public void SetUsage(LineModel model, Usage usage) => FilterSort.SetUsage(model, usage);
        public void SetSorting(LineModel model, Sorting sorting) => FilterSort.SetSorting(model, sorting);
        public void SetFilter(LineModel model, string text) => FilterSort.SetText(model, text);
        public (int, Sorting, Usage, string) GetFilterParms(LineModel model) => FilterSort.GetParms(model);
        #endregion

        #region GetCurrentView  ===============================================
        /// <summary>We are scrolling back and forth in the flattened model hierarchy</summary>
        public (List<LineModel>, LineModel) GetCurrentView(int viewSize, LineModel selected)
        {
            if (_buffer.IsEmpty) RefreshBuffer(selected, viewSize);

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
            var anyChange = false;
            bool isValidSelect = IsValidModel(selected);

            if (isValidSelect)
            {
                switch (change)
                {
                    case ChangeType.OneUp:
                        anyChange = true;                        
                        if (_buffer.IsInvalidOffset(-1))
                            Items[0].FillBufferTraversal(_buffer);
                        break;
                    case ChangeType.TwoUp:
                        anyChange = true;
                        if (_buffer.IsInvalidOffset(-2))
                            Items[0].FillBufferTraversal(_buffer);
                        break;
                    case ChangeType.PageUp:
                        anyChange = true;
                        if (_buffer.IsInvalidOffset(-viewSize))
                            Items[0].FillBufferTraversal(_buffer);
                        break;
                    case ChangeType.OneDown:
                        anyChange = true;
                        if (_buffer.IsInvalidOffset(1))
                            Items[0].FillBufferTraversal(_buffer);
                        break;
                    case ChangeType.TwoDown:
                        anyChange = true;
                        if (_buffer.IsInvalidOffset(2))
                            Items[0].FillBufferTraversal(_buffer);
                        break;
                    case ChangeType.PageDown:
                        anyChange = true;
                        if (_buffer.IsInvalidOffset(viewSize))
                            Items[0].FillBufferTraversal(_buffer);
                        break;
                    case ChangeType.ToggleLeft:
                        anyChange |= selected.ToggleLeft();
                        RefreshBuffer(leading, viewSize);
                        break;
                    case ChangeType.ToggleRight:
                        anyChange |= selected.ToggleRight();
                        RefreshBuffer(leading, viewSize);
                        break;
                    case ChangeType.ToggleFilter:
                        selected.IsFilterVisible = !selected.IsFilterVisible;
                        break;
                    case ChangeType.FilterSortChanged:
                        RefreshBuffer(leading, viewSize);
                        break;
                }
            }

            if (anyChange) PageControl?.Refresh();
        }
        private void RefreshBuffer(LineModel leading, int viewSize)
        {
            _buffer.Initialize(leading, viewSize);
            Items[0].FillBufferTraversal(_buffer);
        }
        #endregion

        #region Validate  =====================================================
        internal void Validate()
        {
            var prev = new Dictionary<Item, LineModel>();
            var anyChange = false;
            foreach (var model in Items)
            {
                anyChange |= model.Validate(this, prev);
            }
            if (anyChange) PageControl?.Refresh();
        }
        #endregion

        #region OverrideMethods  ==============================================
        public override (string, string) GetKindNameId(Root root) => (null, BlankName);
        #endregion
    }
}
