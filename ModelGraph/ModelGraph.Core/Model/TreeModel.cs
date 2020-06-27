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
        public (List<LineModel>, LineModel, bool, bool) GetCurrentView(int viewSize, LineModel leading, LineModel selected)
        {
            if (_buffer.IsEmpty) RefreshBuffer(leading, viewSize);

            var list = _buffer.GetList(_atEnd);
            if (list.Count == 0)
                return (list, null, true, true);

            if (list.Count > viewSize)
                list = list.GetRange(0, viewSize);

            if (IsInvalidModel(selected) || !list.Contains(selected)) selected = null;

            var n = list.Count - 1;
            var eov = _atEnd && list[n] == _endModel;
            var sov = Items[0].Count > 0 && Items[0].Items[0] == list[0];
            if (eov || sov)
            {
                var k = 1;
            }

            return (list, selected, sov, eov);
        }
        bool _atEnd;
        LineModel _endModel;
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
                        RefreshBuffer(-1);
                        break;
                    case ChangeType.TwoUp:
                        anyChange = true;
                        RefreshBuffer(-2);
                        break;
                    case ChangeType.PageUp:
                        anyChange = true;
                        RefreshBuffer(-viewSize);
                        break;
                    case ChangeType.OneDown:
                        anyChange = true;
                        RefreshBuffer(1);
                        break;
                    case ChangeType.TwoDown:
                        anyChange = true;
                        RefreshBuffer(2);
                        break;
                    case ChangeType.PageDown:
                        anyChange = true;
                        RefreshBuffer(viewSize);
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
        private void RefreshBuffer(int scroll)
        {
            if (_buffer.IsInvalidOffset(scroll))
            {
                _atEnd = !Items[0].FillBufferTraversal(_buffer);
                _endModel = _buffer.EndModel;
            }
        }
        private void RefreshBuffer(LineModel leading, int viewSize)
        {
            _buffer.Initialize(leading, viewSize);
            _atEnd = !Items[0].FillBufferTraversal(_buffer);
            _endModel = _buffer.EndModel;
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
