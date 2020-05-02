﻿using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public abstract class LineModel : StoreOf<LineModel>
    {
        public Item Item { get; protected set; }
        private State _state;
        public byte Depth;      // depth of tree hierarchy

        public LineModel ParentModel => Owner as LineModel;

        #region Constructor  ==================================================
        internal LineModel() { }
        internal LineModel(LineModel owner, Item item)
        {
            Item = item;
            Owner = owner;
            Depth = (byte)(owner.Depth + 1);

            owner.Add(this);
        }
        #endregion

        #region State  ========================================================
        [Flags]
        private enum State : ushort
        {
            None = 0,
            HasFailedValueFilter = 0x2000,
            HasFailedUsageFilter = 0x1000,

            IsUsedFilter = 0x800,
            IsNotUsedFilter = 0x400,
            IsAscendingSort = 0x200,
            IsDescendingSort = 0x100,

            IsExpandLeft = 0x80,
            IsExpandRight = 0x40,
            IsFilterFocus = 0x20,
            IsFilterVisible = 0x10,

            HasNoError = 0x8,
            ChangedSort = 0x4,
            ChangedFilter = 0x2,
            IsChanged = 0x1,

            IsExpanded = IsExpandLeft | IsExpandRight,
            IsSorted = IsAscendingSort | IsDescendingSort,
            IsUsageFiltered = IsUsedFilter | IsNotUsedFilter,

            AnyFilterSortChanged = ChangedSort | ChangedFilter,
            SortUsageMode = IsUsageFiltered | IsSorted | ChangedSort | ChangedFilter,
        }
        private bool GetState(State state) => (_state & state) != 0;
        private void SetState(State state, bool value) { if (value) _state |= state; else _state &= ~state; }
        private void SetState(State state, State changedState, bool value) { var prev = GetState(state); if (value) _state |= state; else _state &= ~state; if (prev != value) _state |= changedState; }

        public bool IsChanged { get { return GetState(State.IsChanged); } set { SetState(State.IsChanged, value); } }
        public bool IsFilterFocus { get { return GetState(State.IsFilterFocus); } set { SetState(State.IsFilterFocus, value); } }
        internal bool HasNoError { get { return GetState(State.HasNoError); } set { SetState(State.HasNoError, value); } }

        internal bool HasFailedValueFilter { get { return GetState(State.HasFailedValueFilter); } set { SetState(State.HasFailedValueFilter, value); } }
        internal bool HasFailedUsageFilter { get { return GetState(State.HasFailedUsageFilter); } set { SetState(State.HasFailedUsageFilter, value); } }
        internal bool IsFilteredOut => HasFailedValueFilter || HasFailedUsageFilter;


        public bool IsUsedFilter { get { return GetState(State.IsUsedFilter); } set { SetState(State.IsUsedFilter, State.ChangedFilter, value); } }
        public bool IsNotUsedFilter { get { return GetState(State.IsNotUsedFilter); } set { SetState(State.IsNotUsedFilter, State.ChangedFilter, value); } }


        public bool IsSortAscending { get { return GetState(State.IsAscendingSort); } set { SetState(State.IsAscendingSort, State.ChangedSort, value); } }
        public bool IsSortDescending { get { return GetState(State.IsDescendingSort); } set { SetState(State.IsDescendingSort, State.ChangedSort, value); } }

        public bool IsExpandedLeft { get { return GetState(State.IsExpandLeft); } set { SetState(State.IsExpandLeft, value); if (!value) SetState(State.IsExpandRight, false); } }
        public bool IsExpandedRight { get { return GetState(State.IsExpandRight); } set { SetState(State.IsExpandRight, value); } }

        public bool IsFilterVisible { get { return GetState(State.IsFilterVisible); } set { SetState(State.IsFilterVisible, value); if (value) IsFilterFocus = true; else { ClearViewFilter(); _state |= State.ChangedFilter; } } }

        internal bool IsSorted => GetState(State.IsSorted);
        internal bool IsFiltered => IsUsageFiltered || (IsFilterVisible && HasFilterText);
        internal bool IsExpanded => GetState(State.IsExpanded);
        internal bool IsUsageFiltered => GetState(State.IsUsageFiltered);
        internal bool ChangedSort => GetState(State.ChangedSort);
        internal bool ChangedFilter => GetState(State.ChangedFilter);
        internal bool AnyFilterSortChanged => GetState(State.AnyFilterSortChanged);
        internal void ClearChangedFlags() => _state &= ~State.AnyFilterSortChanged;
        internal void ClearSortUsageMode() => _state &= ~State.SortUsageMode;
        #endregion

        #region CommonMethods   ===============================================
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
        public bool HasFilterText => GetTreeModel().LineModel_FilterSort.TryGetValue(this, out FilterSort fs) && !string.IsNullOrWhiteSpace(fs.FilterSting);
        public string ViewFilter => GetTreeModel().LineModel_FilterSort.TryGetValue(this, out FilterSort fs) ? fs.FilterSting : string.Empty;
        public virtual void ClearViewFilter() => GetTreeModel().LineModel_FilterSort.Remove(this);
        public void UpdateViewFilter(string text) 
        {
            var lineModel_FilterSort = GetTreeModel().LineModel_FilterSort;
            FilterSort fs;
            if (!lineModel_FilterSort.TryGetValue(this, out fs))
                fs = new FilterSort(this);
            fs.FilterSting = text;
        }
        #endregion

        #region Virtual Functions  ============================================
        public byte ItemDelta => (byte)(Item.ChildDelta + Item.ModelDelta);
        public virtual bool CanDrag => false;
        public virtual bool CanSort => false;
        public virtual bool CanFilter => false;
        public virtual bool CanExpandLeft => false;
        public virtual bool CanExpandRight => false;
        public virtual bool CanFilterUsage => false;

        public virtual string GetModelInfo(Chef chef) => default;

        internal virtual (bool, bool) Validate(Chef chef) => (false, false);

        public virtual void GetMenuCommands(Chef chef, List<LineCommand> list) { }
        public virtual void GetButtonCommands(Chef chef, List<LineCommand> list) { }


        public void DragStart(Chef chef) => Chef.DragDropSource = this;
        public virtual DropAction DragEnter(Chef chef) => DropAction.None;

        public virtual DropAction DragDrop(Chef chef) => DropAction.None;
        public virtual DropAction ReorderItems(Chef chef, LineModel target, bool doDrop) => DropAction.None;

        public virtual (string Kind, string Name, int Count, ModelType Type) GetParms(Chef chef) => (null, BlankName, 0, ModelType.Default);

        public virtual Error TryGetError(Chef chef) => default;


        public virtual string GetModelIdentity() =>  $"{ViKey}  ({ItemKey:X3})";
        #endregion
    }
}