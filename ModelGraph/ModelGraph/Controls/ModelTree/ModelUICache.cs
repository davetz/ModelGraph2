
using ModelGraph.Core;
using System.Collections.Generic;
using System.Threading;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Controls
{
    internal class ModelUICache
    {
        private readonly ModelTreeControl TC;
        internal TreeModel TreeRoot { get; private set; }
        internal Canvas TreeCanvas { get; private set; }
        internal LineModel Model { get; private set; }
        internal PropertyModel PropModel {get; private set;}
        internal Root DataRoot { get; private set; }
        internal short ModelDelta { get; private set; }
        internal TextBlock ItemKind { get; private set; }
        internal TextBlock ItemName { get; private set; }
        internal TextBlock ItemInfo { get; private set; }
        internal TextBlock TotalCount { get; private set; }
        internal TextBlock IndentTree { get; private set; }
        internal TextBlock ExpandLeft { get; private set; }
        internal TextBlock ExpandRight { get; private set; }
        internal TextBlock SortMode { get; private set; }
        internal TextBlock UsageMode { get; private set; }
        internal TextBlock FilterMode { get; private set; }
        internal TextBlock FilterCount { get; private set; }
        internal TextBlock PropertyName { get; private set; }
        internal TextBlock ItemHasError { get; private set; }
        internal TextBlock ModelIdentity { get; private set; }

        internal TextBox FilterText { get; private set; }
        internal TextBox TextProperty { get; private set; }
        internal CheckBox CheckProperty { get; private set; }
        internal ComboBox ComboProperty { get; private set; }
        internal Border PropertyBorder { get; private set; }
        internal StackPanel StackPanel { get; private set; }

        #region Constructor  ==================================================
        private ModelUICache(ModelTreeControl tc, Canvas treeCanvas, TreeModel treeRoot, Root dataRoot)
        {
            TC = tc;
            DataRoot = dataRoot;
            TreeRoot = treeRoot;
            TreeCanvas = treeCanvas;
        }
        #endregion

        #region Allocate  =====================================================
        internal static void Allocate(ModelTreeControl tc, Canvas treeCanvas, TreeModel treeRoot, Root dataRoot, int count, Stack<ModelUICache> stack)
        {
            for (int i = 0; i < count; i++)
            {
                stack.Push(new ModelUICache(tc, treeCanvas, treeRoot, dataRoot));
            }
        }
        #endregion

        #region Initialize/Clear  =============================================
        internal void Initialize(LineModel model, int viewIndex)
        {
            Model = model;
            PropModel = model as PropertyModel;
            AddStackPanel(viewIndex);
        }
        internal void Discard() => Clear(true);
        internal void Clear(bool discard = false)
        {
            Model = null;
            PropModel = null;

            ClearItemKind(discard);
            ClearItemName(discard);
            ClearItemInfo(discard);
            ClearTotalCount(discard);
            ClearTreeIndent(discard);
            ClearExpandLeft(discard);
            ClearExpandRight(discard);
            ClearSortMode(discard);
            ClearUsageMode(discard);
            ClearFilterMode(discard);
            ClearFilterText(discard);
            ClearFilterCount(discard);
            ClearPropertyName(discard);
            ClearTextProperty(discard);
            ClearComboProperty(discard);
            ClearCheckProperty(discard);
            ClearItemHasError(discard);
            ClearModelIdentity(discard);
            ClearStackPanel(discard);
        }
        #endregion

        #region AddItemKind  ==================================================
        void AddItemKind(string kind)
        {
            var obj = ItemKind;
            if (obj == null)
            {
                obj = ItemKind = new TextBlock();

                obj.Style = TC.ItemKindStyle;
                obj.DragStarting += TC.ItemName_DragStarting;
                obj.DragOver += TC.ItemName_DragOver;
                obj.Drop += TC.ItemName_Drop;
                obj.PointerEntered += TC.ItemName_PointerEntered;
                ToolTipService.SetToolTip(obj, TC.ItemIdentityTip);
            }

            obj.Text = kind;
            obj.CanDrag = Model.CanDrag;
            obj.AllowDrop = true;
            obj.DataContext = Model;

            StackPanel.Children.Add(obj);
        }
        void ClearItemKind(bool discard)
        {
            var obj = ItemKind;
            if (obj != null)
            {
                obj.DataContext = null;
                if (discard)
                {
                    obj.DragStarting -= TC.ItemName_DragStarting;
                    obj.DragOver -= TC.ItemName_DragOver;
                    obj.Drop -= TC.ItemName_Drop;
                    obj.PointerEntered -= TC.ItemName_PointerEntered;
                    ItemKind = null;
                }
            }
        }
        #endregion

        #region AddItemName  ==================================================
        private void AddItemName(string name)
        {
            var obj = ItemName;
            if (obj == null)
            {
                obj = ItemName = new TextBlock();

                obj.Style = TC.ItemNameStyle;
                obj.DragStarting += TC.ItemName_DragStarting;
                obj.DragOver += TC.ItemName_DragOver;
                obj.Drop += TC.ItemName_Drop;
                obj.PointerEntered += TC.ItemName_PointerEntered;
            }

            obj.Text = name;
            obj.CanDrag = Model.CanDrag;
            obj.AllowDrop = true;
            obj.DataContext = Model;

            StackPanel.Children.Add(obj);
        }
        private void ClearItemName(bool discard)
        {
            var obj = ItemName;
            if (obj != null)
            {
                obj.DataContext = null;
                if (discard)
                {
                    obj.DragStarting -= TC.ItemName_DragStarting;
                    obj.DragOver -= TC.ItemName_DragOver;
                    obj.Drop -= TC.ItemName_Drop;
                    obj.PointerEntered -= TC.ItemName_PointerEntered;

                    ItemName = null;
                }
            }
        }
        #endregion

        #region AddItemInfo  ==================================================
        private void AddItemInfo()
        {
            var obj = ItemInfo;
            if (obj == null)
            {
                obj = ItemInfo = new TextBlock();

                obj.Style = TC.ItemInfoStyle;
            }

            obj.Text = string.Empty;
            obj.DataContext = Model;

            StackPanel.Children.Add(obj);
        }
        private void ClearItemInfo(bool discard)
        {
            var obj = ItemInfo;
            if (obj != null)
            {
                obj.DataContext = null;
                if (discard)
                {
                    ItemInfo = null;
                }
            }
        }
        #endregion

        #region AddTotalCount  ================================================
        void AddTotalCount(int count)
        {
            var obj = TotalCount;
            if (obj == null)
            {
                obj = TotalCount = new TextBlock();

                obj.Style = TC.TotalCountStyle;
                ToolTipService.SetToolTip(obj, TC.TotalCountTip);
            }

            obj.Text = count.ToString();

            StackPanel.Children.Add(obj);
        }
        void ClearTotalCount(bool discard)
        {
            var obj = TotalCount;
            if (obj != null)
            {
                if (discard)
                {
                    TotalCount = null;
                }
            }
        }
        #endregion

        #region AddTreeIndent  ================================================
        private void AddTreeIndent()
        {
            var obj = IndentTree;
            if (obj == null)
            {
                obj = IndentTree = new TextBlock();
                obj.Style = TC.IndentTreeStyle;
            }

            obj.Text = string.Empty;
            obj.DataContext = Model;
            obj.MinWidth = Model.Depth * TC.LevelIndent;

            StackPanel.Children.Add(obj);
        }
        private void ClearTreeIndent(bool discard)
        {
            var obj = IndentTree;
            if (obj != null)
            {
                obj.DataContext = null;
                if (discard)
                {
                    IndentTree = null;
                }
            }
        }
        #endregion

        #region AddExpandLeft  ================================================
        private void AddExpandLeft()
        {
            var obj = ExpandLeft;
            if (obj == null)
            {
                obj = ExpandLeft = new TextBlock();

                obj.Style = TC.ExpanderStyle;
                obj.PointerExited += TC.TextBlockHightlight_PointerExited;
                obj.PointerEntered += TC.TextBlockHighlight_PointerEntered;
                obj.PointerReleased += TC.ExpandTree_PointerReleased;
                ToolTipService.SetToolTip(obj, TC.LeftExpandTip);
            }

            if (Model.CanExpandLeft)
            {
                obj.Text = Model.IsExpandedLeft ? TC.LeftIsExtended : TC.LeftCanExtend;
            }
            else
            {
                obj.Text = string.Empty;
            }

            obj.DataContext = Model;

            StackPanel.Children.Add(obj);
        }
        private void ClearExpandLeft(bool discard)
        {
            var obj = ExpandLeft;
            if (obj != null)
            {
                obj.DataContext = null;
                if (discard)
                {
                    obj.PointerExited -= TC.TextBlockHightlight_PointerExited;
                    obj.PointerEntered -= TC.TextBlockHighlight_PointerEntered;
                    obj.PointerReleased -= TC.ExpandTree_PointerReleased;
                    ExpandLeft = null;
                }
            }
        }
        #endregion

        #region AddExpandRight  ===============================================
        private void AddExpandRight()
        {
            var obj = ExpandRight;
            if (obj == null)
            {
                obj = ExpandRight = new TextBlock();

                obj.Style = TC.ExpanderStyle;
                obj.PointerExited += TC.TextBlockHightlight_PointerExited;
                obj.PointerEntered += TC.TextBlockHighlight_PointerEntered;
                obj.PointerReleased += TC.ExpandChoice_PointerReleased;
                ToolTipService.SetToolTip(obj, TC.RightExpandTip);
            }

            obj.Text = Model.IsExpandedRight ? TC.RightIsExtended : TC.RightCanExtend;
            obj.DataContext = Model;

            StackPanel.Children.Add(obj);
        }
        private void ClearExpandRight(bool discard)
        {
            var obj = ExpandRight;
            if (obj != null)
            {
                obj.DataContext = null;
                if (discard)
                {
                    obj.PointerExited -= TC.TextBlockHightlight_PointerExited;
                    obj.PointerEntered -= TC.TextBlockHighlight_PointerEntered;
                    obj.PointerReleased -= TC.ExpandChoice_PointerReleased;

                    ExpandRight = null;
                }
            }
        }
        #endregion

        #region AddSortMode  ==================================================
        private void AddSortMode(bool canSort)
        {
            var obj = SortMode;
            if (canSort)
            {
                if (obj == null)
                {
                    obj = SortMode = new TextBlock();

                    obj.Style = TC.SortModeStyle;
                    obj.PointerExited += TC.TextBlockHightlight_PointerExited;
                    obj.PointerEntered += TC.TextBlockHighlight_PointerEntered;
                    obj.PointerReleased += TC.SortMode_PointerReleased;
                    ToolTipService.SetToolTip(obj, TC.SortModeTip);
                }
                obj.DataContext = Model;
                obj.Text = Model.IsSortAscending ?
                    TC.SortAscending : (Model.IsSortDescending ? TC.SortDescending : TC.SortNone);

                StackPanel.Children.Add(obj);
            }
            else if (obj != null)
            {
                obj.DataContext = null; // needed for "S" keyboard shortcut (TailButton)
            }
        }
        private void ClearSortMode(bool discard)
        {
            var obj = SortMode;
            if (obj != null)
            {
                obj.DataContext = null;
                if (discard)
                {
                    obj.PointerExited -= TC.TextBlockHightlight_PointerExited;
                    obj.PointerEntered -= TC.TextBlockHighlight_PointerEntered;
                    obj.PointerReleased -= TC.SortMode_PointerReleased;

                    SortMode = null;
                }
            }
        }
        #endregion

        #region AddUsageMode  ==================================================
        private void AddUsageMode(Usage usage)
        {
            var obj = UsageMode;
            if (Model.CanFilterUsage)
            {
                if (obj == null)
                {
                    obj = UsageMode = new TextBlock();

                    obj.Style = TC.UsageModeStyle;
                    obj.PointerExited += TC.TextBlockHightlight_PointerExited;
                    obj.PointerEntered += TC.TextBlockHighlight_PointerEntered;
                    obj.PointerReleased += TC.UsageMode_PointerReleased;
                    ToolTipService.SetToolTip(obj, TC.UsageModeTip);
                }
                obj.DataContext = Model;
                switch (usage)
                {
                    case Usage.IsNotUsed: obj.Text = TC.UsageIsNotUsed; break;
                    case Usage.IsUsed: obj.Text = TC.UsageIsUsed; break;
                    default: obj.Text = TC.UsageAll; break;
                }
                StackPanel.Children.Add(obj);
            }
            else if (obj != null)
            {
                obj.DataContext = null; // needed for "U" keyboard shortcut (TailButton)
            }
        }
        private void ClearUsageMode(bool discard)
        {
            var obj = UsageMode;
            if (obj != null)
            {
                obj.DataContext = null;
                if (discard)
                {
                    obj.PointerExited -= TC.TextBlockHightlight_PointerExited;
                    obj.PointerEntered -= TC.TextBlockHighlight_PointerEntered;
                    obj.PointerReleased -= TC.UsageMode_PointerReleased;

                    UsageMode = null;
                }
            }
        }
        #endregion

        #region AddFilterMode  ================================================
        private void AddFilterMode(bool canFilter)
        {
            var obj = FilterMode;
            if (canFilter)
            {
                if (obj == null)
                {
                    obj = FilterMode = new TextBlock();

                    obj.Style = TC.FilterModeStyle;
                    obj.PointerExited += TC.TextBlockHightlight_PointerExited;
                    obj.PointerEntered += TC.TextBlockHighlight_PointerEntered;
                    obj.PointerReleased += TC.FilterMode_PointerReleased;
                    ToolTipService.SetToolTip(obj, TC.FilterExpandTip);
                }

                obj.DataContext = Model;
                obj.Text = Model.IsFilterVisible ? TC.FilterIsShowing : TC.FilterCanShow;

                StackPanel.Children.Add(obj);
            }
            else if (obj != null)
            {
                obj.DataContext = null; // needed for "F" keyboard shortcut (TailButton)
            }
        }
        private void ClearFilterMode(bool discard)
        {
            var obj = FilterMode;
            if (obj != null)
            {
                obj.DataContext = null;
                if (discard)
                {
                    obj.PointerExited -= TC.TextBlockHightlight_PointerExited;
                    obj.PointerEntered -= TC.TextBlockHighlight_PointerEntered;
                    obj.PointerReleased -= TC.FilterMode_PointerReleased;

                    FilterMode = null;
                }
            }
        }
        #endregion

        #region AddFilterText  ================================================
        private void AddFilterText(string filterText)
        {
            var obj = FilterText;
            if (obj == null)
            {
                obj = FilterText = new TextBox();

                obj.Style = TC.FilterTextStyle;
                obj.KeyDown += TC.FilterText_KeyDown;
                ToolTipService.SetToolTip(obj, TC.FilterTextTip);
            }

            obj.DataContext = Model;
            var str = filterText;
            obj.Text = str;
            obj.Tag = str; //save an initial (unmodified) version of the view filter text

            StackPanel.Children.Add(obj);
        }
        private void ClearFilterText(bool discard)
        {
            var obj = FilterText;
            if (obj != null)
            {
                obj.DataContext = null;
                obj.Tag = null;
                if (discard)
                {
                    obj.KeyDown -= TC.FilterText_KeyDown;

                    FilterText = null;
                }
            }
        }
        #endregion

        #region AddFilterCount  ===============================================
        private void AddFilterCount(int filterCount)
        {
            var obj = FilterCount;
            if (obj == null)
            {
                obj = FilterCount = new TextBlock();

                obj.Style = TC.FilterCountStyle;
                ToolTipService.SetToolTip(obj, TC.FilterCountTip);
            }

            obj.Text = filterCount.ToString();

            StackPanel.Children.Add(obj);
        }
        private void ClearFilterCount(bool discard)
        {
            var obj = FilterCount;
            if (obj != null)
            {
                if (discard)
                {
                    FilterCount = null;
                }
            }
        }
        #endregion

        #region AddPropertyName  ==============================================
        private void AddPropertyName(string name)
        {
            var obj = PropertyName;
            var bdr = PropertyBorder;
            if (obj == null)
            {
                obj = PropertyName = new TextBlock();
                bdr = PropertyBorder = new Border();

                bdr.Style = TC.PropertyBorderStyle;
                bdr.PointerEntered += TC.PropertyBorder_PointerEntered;
                ToolTipService.SetToolTip(bdr, TC.ItemIdentityTip);

                obj.Style = TC.PropertyNameStyle;
                obj.PointerEntered += TC.ItemName_PointerEntered;
                ToolTipService.SetToolTip(obj, TC.ItemIdentityTip);

                bdr.Child = obj;
            }

            bdr.DataContext = Model;
            obj.DataContext = Model;
            obj.Text = name;

            StackPanel.Children.Add(bdr);
        }
        private void ClearPropertyName(bool discard)
        {
            var obj = PropertyName;
            var bdr = PropertyBorder;
            if (obj != null)
            {
                bdr.DataContext = null;
                obj.DataContext = null;
                if (discard)
                {
                    bdr.PointerEntered -= TC.PropertyBorder_PointerEntered;
                    obj.PointerEntered -= TC.ItemName_PointerEntered;

                    PropertyName = null;
                    PropertyBorder = null;
                }
            }
        }
        #endregion

        #region AddTextProperty  ==============================================
        private void AddTextProperty()
        {
            var obj = TextProperty;
            if (obj == null)
            {
                obj = TextProperty = new TextBox();

                obj.Style = TC.TextPropertyStyle;
                obj.KeyDown += TC.TextProperty_KeyDown;
                obj.LostFocus += TC.TextProperty_LostFocus;
                obj.GotFocus += TC.TextProperty_GotFocus;
            }

            obj.DataContext = Model;
            var txt = PropModel.GetTextValue(DataRoot);
            obj.Text = txt ?? string.Empty;
            obj.Tag = obj.Text;
            obj.IsReadOnly = PropModel.IsReadOnly;

            StackPanel.Children.Add(obj);
        }

        private void ClearTextProperty(bool discard)
        {
            var obj = TextProperty;
            if (obj != null)
            {
                obj.DataContext = null;
                obj.Tag = null;
                if (discard)
                {
                    obj.KeyDown -= TC.TextProperty_KeyDown;
                    obj.GotFocus -= TC.TextProperty_GotFocus;
                    obj.LostFocus -= TC.TextProperty_LostFocus;

                    TextProperty = null;
                }
            }
        }
        #endregion

        #region AddCheckProperty  =============================================
        private void AddCheckProperty()
        {
            var obj = CheckProperty;
            if (obj == null)
            {
                obj = CheckProperty = new CheckBox();

                obj.Style = TC.CheckPropertyStyle;
                obj.GotFocus += TC.CheckProperty_GotFocus;
                obj.Checked += TC.CheckProperty_Checked;
                obj.Unchecked += TC.CheckProperty_Checked;
                obj.KeyDown += TC.Check_KeyDown;
            }

            obj.DataContext = Model;
            obj.IsChecked = PropModel.GetBoolValue(DataRoot);

            StackPanel.Children.Add(obj);
        }
        private void ClearCheckProperty(bool discard)
        {
            var obj = CheckProperty;
            if (obj != null)
            {
                obj.DataContext = null;
                if (discard)
                {
                    obj.GotFocus -= TC.CheckProperty_GotFocus;
                    obj.Checked -= TC.CheckProperty_Checked;
                    obj.Unchecked -= TC.CheckProperty_Checked;
                    obj.KeyDown -= TC.Check_KeyDown;

                    CheckProperty = null;
                }
            }
        }
        #endregion

        #region AddComboProperty  =============================================
        private void AddComboProperty()
        {
            var obj = ComboProperty;
            if (obj == null)
            {
                obj = ComboProperty = new ComboBox();

                obj.Style = TC.ComboPropertyStyle;
                obj.GotFocus += TC.ComboProperty_GotFocus;
                obj.SelectionChanged += TC.ComboProperty_SelectionChanged;
                obj.KeyDown += TC.ComboProperty_KeyDown;
            }

            obj.DataContext = Model;
            obj.ItemsSource = PropModel.GetlListValue(DataRoot);
            obj.SelectedIndex = PropModel.GetIndexValue(DataRoot);

            StackPanel.Children.Add(obj);
        }
        private void ClearComboProperty(bool discard)
        {
            var obj = ComboProperty;
            if (obj != null)
            {
                obj.DataContext = null;
                obj.ItemsSource = null;
                if (discard)
                {
                    obj.SelectionChanged -= TC.ComboProperty_SelectionChanged;
                    obj.GotFocus -= TC.ComboProperty_GotFocus;
                    obj.KeyDown -= TC.ComboProperty_KeyDown;

                    ComboProperty = null;
                }
            }
        }
        #endregion

        #region CheckItemHasError  ============================================
        private void CheckItemHasError()
        {
            var error = Model.TryGetError(DataRoot);
            if (error is null) return;

            var obj = ItemHasError;
            if (obj == null)
            {
                obj = ItemHasError = new TextBlock();

                obj.Style = TC.ItemHasErrorStyle;
                obj.Text = TC.ItemHasErrorText;
                obj.PointerExited += TC.TextBlockHightlight_PointerExited;
                obj.PointerEntered += TC.TextBlockHighlight_PointerEntered;
                obj.PointerReleased += TC.ExpandChoice_PointerReleased;
                ToolTipService.SetToolTip(obj, TC.ItemHasErrorTip);
            }

            obj.Tag = error;
            obj.DataContext = Model;

            StackPanel.Children.Add(obj);
        }
        private void ClearItemHasError(bool discard)
        {
            var obj = ItemHasError;
            if (obj != null)
            {
                obj.DataContext = null;
                if (discard)
                {
                    obj.PointerExited -= TC.TextBlockHightlight_PointerExited;
                    obj.PointerEntered -= TC.TextBlockHighlight_PointerEntered;
                    obj.PointerReleased -= TC.ExpandChoice_PointerReleased;

                    ItemHasError = null;
                }
            }
        }
        #endregion

        #region AddModelIdentity  =============================================
        private void AddModelIdentity()
        {
            var obj = ModelIdentity;
            if (obj == null)
            {
                obj = ModelIdentity = new TextBlock();

                obj.Style = TC.ModelIdentityStyle;
                obj.PointerEntered += TC.ModelIdentity_PointerEntered;
            }

            obj.DataContext = Model;

            StackPanel.Children.Add(obj);
        }
        private void ClearModelIdentity(bool discard)
        {
            var obj = ModelIdentity;
            if (obj != null)
            {
                obj.DataContext = null;
                if (discard)
                {
                    obj.PointerEntered -= TC.ModelIdentity_PointerEntered;

                    ModelIdentity = null;
                }
            }
        }
        #endregion

        #region AddStackPanel  ================================================
        private void AddStackPanel(int viewIndex)
        {
            var sp = StackPanel;
            if (sp == null)
            {
                sp = StackPanel = new StackPanel();

                sp.MaxHeight = TC.ElementHieght;
                sp.Orientation = Orientation.Horizontal;

                TreeCanvas.Children.Add(sp);
            }
            Canvas.SetTop(sp, viewIndex * TC.ElementHieght);

            sp.Children.Clear();
            sp.DataContext = Model;
            ModelDelta = Model.ItemDelta;

            var (kind, name) = Model.GetKindNameId(DataRoot);

            AddModelIdentity();
            AddTreeIndent();
            AddExpandLeft();

            if (PropModel != null)
            {
                if (PropModel.IsTextModel)
                {
                    AddPropertyName(name);
                    AddTextProperty();
                    CheckItemHasError();
                    return;
                }
                else if (PropModel.IsCheckModel)
                {
                    AddPropertyName(name);
                    AddCheckProperty();
                    CheckItemHasError();
                    return;
                }
                else if (PropModel.IsComboModel)
                {
                    AddPropertyName(name);
                    AddComboProperty();
                    CheckItemHasError();
                    return;
                }
            }
            else
            {
                var (filterCount, sorting, usage, filterText) = TreeRoot.GetFilterParms(Model);
                AddItemKind(kind);
                AddItemName(name);
                if (Model.CanExpandRight) AddExpandRight();
                CheckItemHasError();

                var count = Model.TotalCount;
                if (count > 0)
                {
                    AddSortMode((Model.CanSort));

                    AddTotalCount(count);
                    AddUsageMode(usage);
                    AddFilterMode(Model.CanFilter);

                    if (Model.CanFilter)
                    {
                        if (Model.IsFilterVisible)
                        {
                            AddFilterText(filterText);
                            AddFilterCount(filterCount);
                        }
                        else if (Model.IsUsedFilter || Model.IsNotUsedFilter)
                        {
                            AddFilterCount(filterCount);
                        }
                    }
                    //if (false)//_root.ModelInfo != null)
                    //{
                    //    AddItemInfo(index, m);
                    //}
                }
            }
        }
        private void ClearStackPanel(bool discard)
        {
            var sp = StackPanel;
            if (sp != null)
            {
                sp.Children.Clear();
                sp.DataContext = null;
                Canvas.SetTop(sp, -4 * TC.ElementHieght);

                if (discard)
                {
                    StackPanel = null;
                }
            }
        }
        #endregion

        #region SetPostion  ==================================================
        internal void SetPosition(int viewIndex)
        {
            Canvas.SetTop(StackPanel, viewIndex * TC.ElementHieght);
        }
        #endregion
    }
}
