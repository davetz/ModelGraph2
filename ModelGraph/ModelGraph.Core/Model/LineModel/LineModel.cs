using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class LineModel : StoreOf<LineModel>
    {
        public Item Item { get; protected set; }
        private ModelState _modelState;
        public byte Depth;      // depth of tree hierarchy

        public LineModel ParentModel => Owner as LineModel;

        #region Constructor  ==================================================
        internal LineModel() { }
        internal LineModel(LineModel owner, Item item)
        {
            Item = item;
            Owner = owner;
            Depth = (byte)(owner.Depth + 1);

            owner.CovertAdd(this);
        }
        #endregion

        #region ModelState  ===================================================
        [Flags]
        private enum ModelState : ushort
        {
            None = 0,

            IsChanged = S1,
            HasNoError = S2,
            ChangedSort = S3,
            ChangedFilter = S4,

            IsExpandLeft = S5,
            IsExpandRight = S6,
            IsFilterFocus = S7,
            IsFilterVisible = S8,

            IsUsedFilter = S9,
            IsNotUsedFilter = S10,
            IsAscendingSort = S11,
            IsDescendingSort = S12,

            HasFailedValueFilter = S13,
            HasFailedUsageFilter = S14,

            IsExpanded = IsExpandLeft | IsExpandRight,
            IsSorted = IsAscendingSort | IsDescendingSort,
            IsUsageFiltered = IsUsedFilter | IsNotUsedFilter,

            AnyFilterSortChanged = ChangedSort | ChangedFilter,
            SortUsageMode = IsUsageFiltered | IsSorted | ChangedSort | ChangedFilter,
        }
        private bool GetState(ModelState state) => (_modelState & state) != 0;
        private void SetState(ModelState state, bool value) { if (value) _modelState |= state; else _modelState &= ~state; }
        private void SetState(ModelState state, ModelState changedState, bool value) { var prev = GetState(state); if (value) _modelState |= state; else _modelState &= ~state; if (prev != value) _modelState |= changedState; }

        public bool IsChanged { get { return GetState(ModelState.IsChanged); } set { SetState(ModelState.IsChanged, value); } }
        public bool IsFilterFocus { get { return GetState(ModelState.IsFilterFocus); } set { SetState(ModelState.IsFilterFocus, value); } }
        internal bool HasNoError { get { return GetState(ModelState.HasNoError); } set { SetState(ModelState.HasNoError, value); } }

        internal bool HasFailedValueFilter { get { return GetState(ModelState.HasFailedValueFilter); } set { SetState(ModelState.HasFailedValueFilter, value); } }
        internal bool HasFailedUsageFilter { get { return GetState(ModelState.HasFailedUsageFilter); } set { SetState(ModelState.HasFailedUsageFilter, value); } }
        internal bool IsFilteredOut => HasFailedValueFilter || HasFailedUsageFilter;


        public bool IsUsedFilter { get { return GetState(ModelState.IsUsedFilter); } set { SetState(ModelState.IsUsedFilter, ModelState.ChangedFilter, value); } }
        public bool IsNotUsedFilter { get { return GetState(ModelState.IsNotUsedFilter); } set { SetState(ModelState.IsNotUsedFilter, ModelState.ChangedFilter, value); } }


        public bool IsSortAscending { get { return GetState(ModelState.IsAscendingSort); } set { SetState(ModelState.IsAscendingSort, ModelState.ChangedSort, value); } }
        public bool IsSortDescending { get { return GetState(ModelState.IsDescendingSort); } set { SetState(ModelState.IsDescendingSort, ModelState.ChangedSort, value); } }

        public bool IsExpandedLeft { get { return GetState(ModelState.IsExpandLeft); } set { SetState(ModelState.IsExpandLeft, value); if (!value) SetState(ModelState.IsExpandRight, false); } }
        public bool IsExpandedRight { get { return GetState(ModelState.IsExpandRight); } set { SetState(ModelState.IsExpandRight, value); } }

        public bool IsFilterVisible { get { return GetState(ModelState.IsFilterVisible); } set { SetState(ModelState.IsFilterVisible, value); if (value) IsFilterFocus = true; else { ClearViewFilter(); _modelState |= ModelState.ChangedFilter; } } }

        internal bool IsSorted => GetState(ModelState.IsSorted);
        internal bool IsFiltered => IsUsageFiltered || (IsFilterVisible && HasFilterText);
        internal bool IsExpanded => GetState(ModelState.IsExpanded);
        internal bool IsUsageFiltered => GetState(ModelState.IsUsageFiltered);
        internal bool ChangedSort => GetState(ModelState.ChangedSort);
        internal bool ChangedFilter => GetState(ModelState.ChangedFilter);
        internal bool AnyFilterSortChanged => GetState(ModelState.AnyFilterSortChanged);
        internal void ClearChangedFlags() => _modelState &= ~ModelState.AnyFilterSortChanged;
        internal void ClearSortUsageMode() => _modelState &= ~ModelState.SortUsageMode;
        #endregion

        #region BufferFillingTraversal  =======================================
        /// <summary>Fill the circular buffer with flattened lineModels, return true if hit end of list</summary>
        internal bool BufferFillingTraversal(CircularBuffer<LineModel> buffer)
        {
            foreach (var child in Items)
            {
                if (child.IsFilteredOut) continue;
                if (buffer.Add(child)) return true; // abort, we are done
                if (child.BufferFillingTraversal(buffer)) return true; // abort, we are done;
            }
            return false; //finished all items with no aborts
        }
        #endregion

        #region CommonMethods   ===============================================
        internal bool IsValidModel(LineModel m) => !IsInvalidModel(m);
        internal bool IsInvalidModel(LineModel m) => IsInvalid(m) || IsInvalid(m.Item);
        internal bool ToggleLeft() => IsExpandedLeft ? CollapseLeft() : ExpandLeft();
        internal bool ToggleRight() => IsExpandedRight ? CollapseRight() : ExpandRight();

        protected bool CollapseLeft()
        {
            IsExpandedLeft = false;
            IsExpandedRight = false;
            if (Count == 0) return false;

            DiscardChildren();
            return true;
        }
        protected bool CollapseRight()
        {
            var anyChange = false;
            if (IsExpandedRight)
            {
                int N = Count;
                for (int i = 0; i < N; i++)
                {
                    var item = Items[0];
                    if (item is PropertyModel)
                    {
                        anyChange = true;
                        CovertRemove(item);
                        item.Discard();
                    }
                    else
                        break;
                }
                IsExpandedRight = false;
            }
            return anyChange;
        }

        public int FilterCount => GetFilterCount();
        private int GetFilterCount()
        {
            int count = 0;
            foreach (var line in Items)
            {
                if (line.IsFilteredOut) continue;
                count++;
            }
            return count;
        }
        /// <summary>Walk up item tree hierachy to find the parent RootTreeModel</summary>
        public RootTreeModel RootTreeModel => GetRootTreeModel();
        private RootTreeModel GetRootTreeModel()
        {
            var item = this;
            while (item != null)
            {
                if (item is RootTreeModel root) return root;
                item = item.Owner as LineModel;
            }
            throw new Exception("GetRootTreeModel: Corrupted item hierarchy"); // I seriously hope this never happens
        }
        /// <summary>Walk up item tree hierachy to find the parent TreeModel</summary>
        public TreeModel TreeModel => GetTreeModel();
        private TreeModel GetTreeModel()
        {
            var mdl = this;
            for (int i = 0; i < 100; i++)
            {
                if (mdl is null) break;
                if (mdl is TreeModel root) return root;
                mdl = mdl.Owner as LineModel;
            }
            throw new Exception("LineModel GetTreeModel() - Encountered a corrupt model hierarchy");
        }
        public bool HasFilterText => GetTreeModel().LineModel_FilterSort.TryGetValue(this, out string fs) && !string.IsNullOrWhiteSpace(fs);
        public string ViewFilter => GetTreeModel().LineModel_FilterSort.TryGetValue(this, out string fs) ? fs : string.Empty;
        public virtual void ClearViewFilter() => GetTreeModel().LineModel_FilterSort.Remove(this);
        public void UpdateViewFilter(string text)
        {
            var lineModel_FilterSort = GetTreeModel().LineModel_FilterSort;
            
            lineModel_FilterSort[this] = text;
        }
        #endregion

        #region Virtual Functions  ============================================
        internal virtual bool ExpandLeft() => false;
        internal virtual bool ExpandRight() => false;

        public virtual (string kind, string name, int count) GetLineParms(Chef chef)
        {
            var (kind, name) = GetKindNameId(chef);
            return (kind, name, Count);
        }
        public byte ItemDelta => (byte)(Item.ChildDelta + Item.ModelDelta);
        public virtual bool CanDrag => false;
        public virtual bool CanSort => false;
        public virtual bool CanFilter => false;
        public virtual bool CanExpandLeft => false;
        public virtual bool CanExpandRight => false;
        public virtual bool CanFilterUsage => false;

        public virtual string GetModelInfo(Chef chef) => default;

        public virtual void GetMenuCommands(Chef chef, List<LineCommand> list) { }
        public virtual void GetButtonCommands(Chef chef, List<LineCommand> list) { }


        public void DragStart(Chef chef) => Chef.DragDropSource = this;
        public virtual DropAction DragEnter(Chef chef) => DropAction.None;

        public virtual DropAction DragDrop(Chef chef) => DropAction.None;
        public virtual DropAction ReorderItems(Chef chef, LineModel target, bool doDrop) => DropAction.None;

        public virtual Error TryGetError(Chef chef) => default;

        public virtual string GetModelIdentity() =>  $"{IdKey}  ({ItemKey:X3})";

        internal virtual bool Validate(Dictionary<Item, LineModel> prev)
        {
            var anyChange = false;
            foreach (var child in Items)
            {
                anyChange |= child.Validate(prev);
            }
            return anyChange;
        }
        #endregion
    }
}
