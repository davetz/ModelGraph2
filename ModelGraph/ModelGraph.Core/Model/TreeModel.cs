using System;
using System.Collections.Generic;
using Windows.Media.Devices.Core;

namespace ModelGraph.Core
{
    /// <summary>Flat list of LineModel that emulates a UI tree view</summary>
    public abstract class TreeModel : LineModel, IRootModel
    {
        public Item RootItem => Item;
        public IPageControl PageControl { get; set; } // reference the UI PageControl       
        public ControlType ControlType { get; private set; }

        public string TitleName => DataRoot.TitleName;
        public string TitleSummary => DataRoot.TitleSummary;

        #region Constructor  ==================================================
        internal TreeModel(Root root) //==================================== invoked in the RootModel constructor
        {
            Owner = Item = root;
            Depth = 254;
            ControlType = ControlType.PrimaryTree;

            new RootModel_612(this, root);

            root.Add(this);
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

        #region ValidateBuffer  ===============================================
        private CircularBuffer<LineModel> _buffer;

        /// <summary>Ensure buffer is not null and large enough, return true if new buffer</summary>
        private bool ValidateBuffer(int viewSize)
        {
            var size = viewSize * 3;
            if (_buffer is null || size > _buffer.Size)
            {
                _buffer = new CircularBuffer<LineModel>(size);
                return true;
            }
            return false;
        }
        #endregion

        #region FilterParms  ==================================================
        public void SetUsage(LineModel model, Usage usage) => FilterSort.SetUsage(model, usage);
        public void SetSorting(LineModel model, Sorting sorting) => FilterSort.SetSorting(model, sorting);
        public void SetFilter(LineModel model, string text) => FilterSort.SetText(model, text);
        public (int, Sorting, Usage, string) GetFilterParms(LineModel model) => FilterSort.GetParms(model);
        #endregion

        #region GetCurrentView  ===============================================
        private int GetScaledSize(int viewSize) => viewSize * 3;
        /// <summary>We are scrolling back and forth in the flattened model hierarchy</summary>
        public (List<LineModel>, LineModel) GetCurrentView(int viewSize, LineModel selected)
        {
            if (ValidateBuffer(viewSize))
                RefreshBuffer();

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

            ValidateBuffer(viewSize);
            if (isValidSelect)
            {
                switch (change)
                {
                    case ChangeType.OneDown:
                        RefreshBuffer(leading);
                        break;
                    case ChangeType.ToggleLeft:
                        anyChange |= selected.ToggleLeft();
                        RefreshBuffer(leading);
                        break;
                    case ChangeType.ToggleRight:
                        anyChange |= selected.ToggleRight();
                        RefreshBuffer(leading);
                        break;
                    case ChangeType.ToggleFilter:
                        selected.IsFilterVisible = !selected.IsFilterVisible;
                        break;
                    case ChangeType.FilterSortChanged:
                        RefreshBuffer(leading);
                        break;
                }
            }

            if (anyChange) PageControl?.Refresh();
        }
        private void RefreshBuffer(LineModel leading = null)
        {
            if (IsValidModel(leading)) _buffer.Initialize(leading);
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
