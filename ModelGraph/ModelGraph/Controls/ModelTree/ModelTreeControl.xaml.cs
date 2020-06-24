using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using ModelGraph.Core;
using ModelGraph.Helpers;
using ModelGraph.Services;
using Windows.System;
using System.Runtime.CompilerServices;

namespace ModelGraph.Controls
{
    public sealed partial class ModelTreeControl : Page, IPageControl, IModelPageControl
    {
        #region Fields  =======================================================
        Root _dataRoot;
        TreeModel _treeRoot;
        LineModel _selected;

        List<LineModel> _viewList = new List<LineModel>();
        List<LineCommand> _menuCommands = new List<LineCommand>();
        List<LineCommand> _buttonCommands = new List<LineCommand>();

        internal ToolTip ItemIdentityTip { get; private set; }
        internal ToolTip ModelIdentityTip { get; private set; }

        internal int LevelIndent { get; private set; }
        internal int ElementHieght { get; private set; }

        internal Style ExpanderStyle { get; private set; }
        internal Style ItemKindStyle { get; private set; }
        internal Style ItemNameStyle { get; private set; }
        internal Style ItemInfoStyle { get; private set; }
        internal Style SortModeStyle { get; private set; }
        internal Style UsageModeStyle { get; private set; }
        internal Style TotalCountStyle { get; private set; }
        internal Style IndentTreeStyle { get; private set; }
        internal Style FilterModeStyle { get; private set; }
        internal Style FilterTextStyle { get; private set; }
        internal Style FilterCountStyle { get; private set; }
        internal Style ItemHasErrorStyle { get; private set; }
        internal Style PropertyNameStyle { get; private set; }
        internal Style TextPropertyStyle { get; private set; }
        internal Style CheckPropertyStyle { get; private set; }
        internal Style ComboPropertyStyle { get; private set; }
        internal Style ModelIdentityStyle { get; private set; }
        internal Style PropertyBorderStyle { get; private set; }

        ToolTip[] _menuItemTips;
        ToolTip[] _itemButtonTips;

        Button[] _itemButtons;
        MenuFlyoutItem[] _menuItems;
        int _menuItemsCount;

        int Count => (_viewList == null) ? 0 : _viewList.Count;

        // segoe ui symbol font glyphs  =====================
        internal string LeftCanExtend => "\u25b7";
        internal string LeftIsExtended => "\u25e2";

        internal string RightCanExtend => "\u25c1";
        internal string RightIsExtended => "\u25e3";

        internal string SortNone => "\u2012";
        internal string SortAscending => "\u2228";
        internal string SortDescending => "\u2227";

        internal string UsageAll => "A";
        internal string UsageIsUsed => "U";
        internal string UsageIsNotUsed => "N";

        internal string FilterCanShow => "\uE71C";
        internal string FilterIsShowing => "\uE71C\uEBE7";

        internal string ItemHasErrorText => "\uE783";

        internal string SortModeTip { get; private set; }
        internal string UsageModeTip { get; private set; }
        internal string LeftExpandTip { get; private set; }
        internal string TotalCountTip { get; private set; }
        internal string FilterTextTip { get; private set; }
        internal string FilterCountTip { get; private set; }
        internal string RightExpandTip { get; private set; }
        internal string FilterExpandTip { get; private set; }
        internal string ItemHasErrorTip { get; private set; }

        // position all unused cache elements offScreen
        const int notVisible = 32767;
        #endregion

        #region Constructor  ==================================================
        public ModelTreeControl(TreeModel root)
        {
            if (root is null) throw new NullReferenceException();
            _treeRoot = root;
            _dataRoot = root.DataRoot;

            InitializeComponent();
            Initialize();
            Loaded += ModelTreeControl_Loaded;
        }
        private void ModelTreeControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= ModelTreeControl_Loaded;
            InitializeCache();
            Refresh();
        }
        #endregion

        #region LineModelCache  ===============================================
        Stack<LineModelCache> _freeCache = new Stack<LineModelCache>(30);
        Dictionary<LineModel, LineModelCache> _model_cache = new Dictionary<LineModel, LineModelCache>(31);
        HashSet<LineModel> _oldModels = new HashSet<LineModel>();

        private void InitializeCache()
        {
            LineModelCache.Allocate(this, TreeCanvas, _treeRoot, _dataRoot, 31, _freeCache);
        }
        private void ClearCache()
        {
            while (_freeCache.Count > 0)
            {
                var fc = _freeCache.Pop();
                fc.Discard();
            }
            _freeCache.Clear();
            foreach (var lc in _model_cache.Values)
            {
                lc.Discard();
            }
            _model_cache.Clear();
        }
        private void ValidateCache(List<LineModel> newModels)
        {
            _oldModels.Clear();
            foreach (var m in _model_cache.Keys) { _oldModels.Add(m); }

            for (int i = 0; i < newModels.Count; i++)
            {
                var m = newModels[i];
                if (_model_cache.TryGetValue(m, out LineModelCache mc))
                {
                    mc.SetPosition(i);
                    _oldModels.Remove(m);
                }
                else
                {
                    if (_freeCache.Count == 0) LineModelCache.Allocate(this, TreeCanvas, _treeRoot, _dataRoot, 11, _freeCache);
                    var nc = _freeCache.Pop();
                    nc.Initialize(m, i);
                    _model_cache.Add(m, nc);
                }
            }
            foreach (var m in _oldModels)
            {
                if (!_model_cache.TryGetValue(m, out LineModelCache oc))
                    throw new Exception("ValidateCache corrupt model_cache");
                oc.Clear();
                _freeCache.Push(oc);
            }
        }
        private LineModelCache GetCache(LineModel m)
        {
            if (!_model_cache.TryGetValue(m, out LineModelCache lc))
                throw new Exception("ValidateCache corrupt model_cache");
            return lc;
        }
        #endregion


        #region PostRefreshViewList  ==========================================
        private async System.Threading.Tasks.Task PostRefreshViewListAsync(ChangeType change = ChangeType.None)
        {
            LineModel leading = (_viewList is null || _viewList.Count == 0) ? null : _viewList[0];
            //ResetCacheDelta(_selected);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { _treeRoot.RefreshViewList(ViewSize, leading, _selected, change); });
            Refresh();
        }
        private async System.Threading.Tasks.Task PostSetUsageAsync(LineModel model, Usage usage)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { _treeRoot.SetUsage(model, usage); });
            _ = PostRefreshViewListAsync(ChangeType.FilterSortChanged);
        }
        private async System.Threading.Tasks.Task PostSetSortingAsync(LineModel model, Sorting sorting)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { _treeRoot.SetSorting(model, sorting); });
            _ = PostRefreshViewListAsync(ChangeType.FilterSortChanged);
        }
        private async System.Threading.Tasks.Task PostSetFilterAsync(LineModel model, string text)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { _treeRoot.SetFilter(model, text); });
            _ = PostRefreshViewListAsync(ChangeType.FilterSortChanged);
        }
        #endregion

        #region SetSize  ======================================================
        public void SetSize(double width, double height)
        {
            if (_treeRoot is null || TreeCanvas is null) return;
            if (height > 0)
            {
                TreeCanvas.Width = Width = width;
                TreeCanvas.Height = Height = height;

                _ = PostRefreshViewListAsync();
            }
        }
        int ViewSize => (int)(Height / ElementHieght);
        #endregion

        #region IPageControl  =================================================
        public void CreateNewPage(IRootModel model, ControlType ctlType)
        {
            if (model is null) return;
            _ = ModelPageService.Current.CreateNewPageAsync(model, ctlType);
        }
        public IRootModel IModel => _treeRoot;
        #endregion

        #region IModelControl  ================================================
        public void Apply()
        {
        }
        public void Revert()
        {
        }
        public (int Width, int Height) PreferredSize => (400, 320);
        #endregion

        #region Initialize  ===================================================
        void Initialize()
        {

            ItemIdentityTip = new ToolTip();
            ItemIdentityTip.Opened += ItemIdentityTip_Opened;

            ModelIdentityTip = new ToolTip();
            ModelIdentityTip.Opened += ModelIdentityTip_Opened;

            LevelIndent = (int)(Resources["LevelIndent"] as Double?).Value;
            ElementHieght = (int)(Resources["ElementHieght"] as Double?).Value;

            ExpanderStyle = Resources["ExpanderStyle"] as Style;
            ItemKindStyle = Resources["ItemKindStyle"] as Style;
            ItemNameStyle = Resources["ItemNameStyle"] as Style;
            ItemInfoStyle = Resources["ItemInfoStyle"] as Style;
            SortModeStyle = Resources["SortModeStyle"] as Style;
            UsageModeStyle = Resources["UsageModeStyle"] as Style;
            TotalCountStyle = Resources["TotalCountStyle"] as Style;
            IndentTreeStyle = Resources["IndentTreeStyle"] as Style;
            FilterModeStyle = Resources["FilterModeStyle"] as Style;
            FilterTextStyle = Resources["FilterTextStyle"] as Style;
            FilterCountStyle = Resources["FilterCountStyle"] as Style;
            ItemHasErrorStyle = Resources["ItemHasErrorStyle"] as Style;
            PropertyNameStyle = Resources["PropertyNameStyle"] as Style;
            TextPropertyStyle = Resources["TextPropertyStyle"] as Style;
            CheckPropertyStyle = Resources["CheckPropertyStyle"] as Style;
            ComboPropertyStyle = Resources["ComboPropertyStyle"] as Style;
            ModelIdentityStyle = Resources["ModelIdentityStyle"] as Style;
            PropertyBorderStyle = Resources["PropertyBorderStyle"] as Style;

            SortModeTip = "010S".GetLocalized();
            UsageModeTip = "00ES".GetLocalized();
            LeftExpandTip = "006S".GetLocalized();
            TotalCountTip = "007S".GetLocalized();
            FilterTextTip = "008S".GetLocalized();
            FilterCountTip = "009S".GetLocalized();
            RightExpandTip = "00AS".GetLocalized();
            FilterExpandTip = "00BS".GetLocalized();
            ItemHasErrorTip = "00FS".GetLocalized();

            _itemButtons = new Button[]
            {
                ItemButton1,
                ItemButton2,
                ItemButton3
            };
            _menuItems = new MenuFlyoutItem[]
            {
                MenuItem1,
                MenuItem2,
                MenuItem3,
                MenuItem4,
                MenuItem5,
                MenuItem6,
            };

            _itemButtonTips = new ToolTip[_itemButtons.Length];
            for (int i = 0; i < _itemButtons.Length; i++)
            {
                var tip = new ToolTip();
                _itemButtonTips[i] = tip;
                ToolTipService.SetToolTip(_itemButtons[i], tip);
            }

            _menuItemTips = new ToolTip[_menuItems.Length];
            for (int i = 0; i < _menuItems.Length; i++)
            {
                var tip = new ToolTip();
                _menuItemTips[i] = tip;
                ToolTipService.SetToolTip(_menuItems[i], tip);
            }
        }
        #endregion

        #region Release  ======================================================
        public void Release()
        {
            ClearCache();

            TreeCanvas.Children.Clear();
            TreeCanvas = null;

            ItemIdentityTip.Opened -= ItemIdentityTip_Opened;
            ModelIdentityTip.Opened -= ModelIdentityTip_Opened;

            _treeRoot = null;
            _selected = null;
            _viewList.Clear();
            _menuCommands.Clear();
            _buttonCommands.Clear();
            ItemIdentityTip = null;
            ModelIdentityTip = null;
        }
        #endregion

        #region KeyboardAccelerators  =========================================
        private void KeyPageUp_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            _ = PostRefreshViewListAsync(ChangeType.PageUp);
            args.Handled = true;
        }
        private void KeyPageDown_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            _ = PostRefreshViewListAsync(ChangeType.PageDown);
            args.Handled = true;
        }

        private void KeyUp_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            TryGetPrevModel();
            args.Handled = true;
        }
        private void KeyDown_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            TryGetNextModel();
            args.Handled = true;
        }

        private void KeyEnd_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            _ = PostRefreshViewListAsync(ChangeType.GoToBottom);
            args.Handled = true;
        }
        private void KeyHome_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            _ = PostRefreshViewListAsync(ChangeType.GoToTop);
            args.Handled = true;
        }

        private void KeyLeft_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_selected.CanExpandLeft)
            {
                _ = PostRefreshViewListAsync(ChangeType.ToggleLeft);
            }
            args.Handled = true;
        }
        private void KeyRight_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_selected.CanExpandRight)
            {
                _ = PostRefreshViewListAsync(ChangeType.ToggleRight);
            }
            args.Handled = true;
        }
        private void KeyEnter_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            TrySetControlFocus();
            args.Handled = true;
        }

        private void KeyEscape_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            SetDefaultFocus();
            if (_selected != null)
            {
                _selected.IsFilterVisible = false;
                _selected.IsExpandedLeft = false;
                _ = PostRefreshViewListAsync(ChangeType.FilterSortChanged);
            }
            args.Handled = true;
        }
        private void KeyMenu_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;
        }
        void TryGetPrevModel()
        {
            SetDefaultFocus();
            if (_selected is null)
            {
                if (_viewList is null || _viewList.Count == 0) return;
                _selected = _viewList[0];
                RefreshSelect();
            }
            else
            {
                var i = _viewList.IndexOf(_selected) - 1;
                if (i >= 0 && i < _viewList.Count)
                {
                    _selected = _viewList[i];
                    RefreshSelect();
                }
                else
                    _ = PostRefreshViewListAsync(ChangeType.OneUp);
            }
        }
        void TryGetNextModel()
        {
            SetDefaultFocus();
            if (_selected is null)
            {
                if (_viewList is null || _viewList.Count == 0) return;
                _selected = _viewList[0];
                RefreshSelect();
            }
            else
            {
                var i = _viewList.IndexOf(_selected) + 1;
                if (i > 0 && i < _viewList.Count)
                {
                    _selected = _viewList[i];
                    RefreshSelect();
                }
                else
                    _ = PostRefreshViewListAsync(ChangeType.OneDown);
            }
        }
        #endregion

        #region AppButton_Click  ==============================================
        private void AppButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var cmd = btn.Tag as LineCommand;
            cmd.Execute();
        }
        #endregion

        #region Button_Click  =================================================
        void ItemButton_Click(object sender, RoutedEventArgs e)
        {
            var obj = sender as Button;
            var cmd = obj.DataContext as LineCommand;
            cmd.Execute();
        }
        void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var obj = sender as MenuFlyoutItem;
            var cmd = obj.DataContext as LineCommand;
            cmd.Execute();
        }
        #endregion

        #region PointerWheelChanged  ==========================================
        bool _pointWheelEnabled;
        void TreeCanvas_PointerWheelChanged(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            e.Handled = true;
            if (_pointWheelEnabled)
            {
                var cp = e.GetCurrentPoint(TreeCanvas);
                if (cp.Properties.MouseWheelDelta < 0)
                    _ = PostRefreshViewListAsync(ChangeType.TwoUp);
                else
                    _ = PostRefreshViewListAsync(ChangeType.TwoDown);
            }
        }
        #endregion

        #region ToolTip_Opened  ===============================================
        void ItemIdentityTip_Opened(object sender, RoutedEventArgs e)
        {
            var tip = sender as ToolTip;
            var mdl = tip.DataContext as LineModel;
            var content = mdl.GetSummaryId(_dataRoot);
            tip.Content = string.IsNullOrWhiteSpace(content) ? null : content;
        }
        void ModelIdentityTip_Opened(object sender, RoutedEventArgs e)
        {
            var tip = sender as ToolTip;
            var mdl = tip.DataContext as LineModel;
            tip.Content = mdl.GetModelIdentity();
        }
        #endregion

        #region MenuFlyout_Opening  ===========================================
        void MenuFlyout_Opening(object sender, object e)
        {
            var fly = sender as MenuFlyout;
            fly.Items.Clear();
            for (int i = 0; i < _menuItemsCount; i++)
            {
                fly.Items.Add(_menuItems[i]);
            }
        }

        #endregion

        #region PointerPressed  ===============================================
        void TreeGrid_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _selected = PointerModel(e);
            SetDefaultFocus();
            RefreshSelect();
            e.Handled = true;
        }
        LineModel PointerModel(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(TreeCanvas);
            var i = (int)(p.Position.Y / ElementHieght);
            return (Count == 0) ? null : (i < 0) ? _viewList[0] : (i < Count) ? _viewList[i] : _viewList[Count - 1];
        }
        #endregion


        #region Refresh  ======================================================
        public void Refresh()
        {
            var (viewList, selected) = _treeRoot.GetCurrentView(ViewSize, _selected);
            _viewList = viewList;
            _selected = selected;
            var N = _viewList.Count;

            _pointWheelEnabled = false;

            ValidateCache(_viewList);

            RefreshSelect();

            _pointWheelEnabled = true;
        }
        #endregion

        #region RefreshRoot  ==================================================
        private void RefreshRoot()
        {
            var buttonCommands = new List<LineCommand>();
           // _root.PageButtonComands(buttonCommands);

            var N = buttonCommands.Count;
            var M = ControlPanel.Children.Count;
            for (int i = 0; i < M; i++)
            {
                if (ControlPanel.Children[i] is Button btn)
                {
                    if (i < N)
                    {
                        var cmd = buttonCommands[i];
                        btn.Tag = cmd;
                        btn.Content = cmd.GetSingleNameId(_dataRoot);
                        btn.Visibility = Visibility.Visible;
                        ToolTipService.SetToolTip(btn, cmd.GetSummaryId(_dataRoot));
                    }
                    else
                    {
                        btn.Visibility = Visibility.Collapsed;
                    }
                }
            }
            ModelTitle.Text = _treeRoot.TitleName;
           // _root.IsChanged = false;
        }

        #endregion

        #region RefreshSelect  ================================================
        void RefreshSelect(bool restoreFocus = true)
        {
            TreeCanvas.KeyboardAccelerators.Clear();
            _acceleratorKeyCommands.Clear();

            _sortControl = _filterControl = null;

            if (Count == 0 || _selected == null)
            {
                foreach (var btn in _itemButtons) { btn.Visibility = Visibility.Collapsed; }

                return;
            }

            
            var lc = GetCache(_selected);

            if (lc.SortMode != null && lc.SortMode.DataContext != null)
            {
                _sortControl = lc.SortMode;
                var acc = new KeyboardAccelerator { Key = VirtualKey.S, Modifiers = VirtualKeyModifiers.Control};
                acc.Invoked += Accelerator_SortMode_Invoked;
                TreeCanvas.KeyboardAccelerators.Add(acc);
            }

            if (lc.UsageMode != null && lc.UsageMode.DataContext != null)
            {
                _usageControl = lc.UsageMode;
                var acc = new KeyboardAccelerator { Key = VirtualKey.U, Modifiers = VirtualKeyModifiers.Control };
                acc.Invoked += Accelerator_UsageMode_Invoked;
                TreeCanvas.KeyboardAccelerators.Add(acc);
            }

            if (_selected.CanDrag)
            {
                var acc = new KeyboardAccelerator { Key = VirtualKey.C, Modifiers = VirtualKeyModifiers.Control };
                acc.Invoked += Accelerator_ModelCopy_Invoked;
                TreeCanvas.KeyboardAccelerators.Add(acc);
            }

            if (_selected.DragEnter(_dataRoot) != DropAction.None)
            {
                var acc = new KeyboardAccelerator { Key = VirtualKey.V, Modifiers = VirtualKeyModifiers.Control };
                acc.Invoked += Accelerator_ModelPaste_Invoked;
                TreeCanvas.KeyboardAccelerators.Add(acc);
            }

            if (lc.FilterMode != null && lc.FilterMode.DataContext != null)
            {
                _filterControl = lc.FilterMode;
            }


            if (_selected.GetDescriptionId(_dataRoot) != null)
            {
                HelpButton.Visibility = Visibility.Visible;
                PopulateItemHelp(_selected.GetDescriptionId(_dataRoot));
            }
            else
            {
                HelpButton.Visibility = Visibility.Collapsed;
            }

            _selected.GetMenuCommands(_dataRoot, _menuCommands);
            _selected.GetButtonCommands(_dataRoot, _buttonCommands);

            var cmds = _buttonCommands;
            var len1 = cmds.Count;
            var len2 = _itemButtons.Length;

            for (int i = 0; i < len2; i++)
            {
                if (i < len1)
                {
                    var cmd = _buttonCommands[i];
                    _itemButtons[i].DataContext = cmd;
                    _itemButtons[i].Content = cmd.GetSingleNameId(_dataRoot);
                    _itemButtonTips[i].Content = cmd.GetSummaryId(_dataRoot);
                    _itemButtons[i].Visibility = Visibility.Visible;
                    var key = cmd.GetAcceleratorId(_dataRoot);
                    if (cmd.IsInsertCommand)
                    {
                        var acc = new KeyboardAccelerator { Key = VirtualKey.Insert };
                        acc.Invoked += Accelerator_Invoked;
                        _acceleratorKeyCommands.Add(acc, cmd);
                        TreeCanvas.KeyboardAccelerators.Add(acc);
                    }
                    else if (cmd.IsRemoveCommand)
                    {
                        var acc = new KeyboardAccelerator { Key = VirtualKey.Delete };
                        acc.Invoked += Accelerator_Invoked;
                        _acceleratorKeyCommands.Add(acc, cmd);
                        TreeCanvas.KeyboardAccelerators.Add(acc);
                    }
                    else if (_virtualKeys.TryGetValue(key, out VirtualKey vkey))
                    {
                        var acc = new KeyboardAccelerator { Key = vkey, Modifiers = VirtualKeyModifiers.Control };
                        acc.Invoked += Accelerator_Invoked;
                        _acceleratorKeyCommands.Add(acc, cmd);
                        TreeCanvas.KeyboardAccelerators.Add(acc);
                    }
                }
                else
                {
                    _itemButtons[i].Visibility = Visibility.Collapsed;
                }
            }

            cmds = _menuCommands;
            if (cmds.Count == 0)
            {
                MenuButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MenuButton.Visibility = Visibility.Visible;

                _menuItemsCount = len1 = cmds.Count;
                len2 = _menuItems.Length;

                for (int i = 0; i < len2; i++)
                {
                    if (i < len1)
                    {
                        var cmd = cmds[i];
                        _menuItems[i].DataContext = cmd;
                        _menuItems[i].Text = cmd.GetSingleNameId(_dataRoot);
                        _menuItemTips[i].Content = cmd.GetSummaryId(_dataRoot);
                    }
                }
            }

            if (restoreFocus) TryRestoreFocus();
        }
        #endregion

        //
        #region TrySetControlFocus  ===========================================
        // given the focusModel try to determine what is the most
        // logical text box to enter, then set the keyboard focus to it,
        // otherwise set focus to our reliable dummy FocusButton
        private void TrySetControlFocus(LineModel focusModel = null)
        {
            if (focusModel != null) _selected = focusModel;

            var lc = GetCache(_selected);

            if (_selected.CanFilter)
            {
                if (_selected.IsFilterVisible)
                {
                    _selected.IsFilterFocus = false;
                    SetFocus(lc.FilterText);
                }
                else
                {
                    _tryAfterRefresh = true;
                    _ = PostRefreshViewListAsync(ChangeType.ToggleFilter);
                }
            }
            else if (_selected is PropertyModel pm)
            {
                if (pm.IsTextModel) SetFocus(lc.TextProperty);
                else if (pm.IsCheckModel) SetFocus(lc.CheckProperty);
                else if (pm.IsComboModel) SetFocus(lc.ComboProperty);
            }
            else
                SetFocus(FocusButton);


            void SetFocus(Control ctrl)
            {
                _focusControl = ctrl;
                _tryAfterRefresh = false;
                ctrl.Focus(FocusState.Keyboard);
            }
        }
        private void TryRestoreFocus()
        {
            if (_tryAfterRefresh || _focusControl != FocusButton)
                TrySetControlFocus();
            else
                SetDefaultFocus();
        }
        private void SetDefaultFocus()
        {
            _focusControl = FocusButton;
            _tryAfterRefresh = false;
            FocusButton.Focus(FocusState.Keyboard);
        }
        bool _tryAfterRefresh;
        Control _focusControl;
        #endregion
        //
        #region AcceleratorKeyCommands  =======================================
        private void Accelerator_ModelCopy_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_selected.CanDrag)
                _selected.DragStart(_dataRoot);
            args.Handled = true;
        }
        private void Accelerator_ModelPaste_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_selected.DragEnter(_dataRoot) != DropAction.None)
            _selected.DragDrop(_dataRoot);
            args.Handled = true;
        }
        private void Accelerator_SortMode_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_sortControl != null) ExecuteSort(_sortControl);
            args.Handled = true;
        }
        private void Accelerator_UsageMode_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_usageControl != null) ExecuteUsage(_usageControl);
            args.Handled = true;
        }
        private void Accelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_acceleratorKeyCommands.TryGetValue(sender, out LineCommand cmd)) cmd.Execute();
            args.Handled = true;
        }
        private readonly Dictionary<KeyboardAccelerator, LineCommand> _acceleratorKeyCommands = new Dictionary<KeyboardAccelerator, LineCommand>();

        private readonly List<KeyboardAccelerator> menuCommandAccelerators = new List<KeyboardAccelerator>(4);
        private readonly List<KeyboardAccelerator> buttonCommandAccelerators = new List<KeyboardAccelerator>(4);

        static readonly Dictionary<string, Windows.System.VirtualKey> _virtualKeys = new Dictionary<string, VirtualKey>
        {
            ["A"] = VirtualKey.A,
            ["B"] = VirtualKey.B,
            ["C"] = VirtualKey.C,
            ["D"] = VirtualKey.D,
            ["E"] = VirtualKey.E,
            ["F"] = VirtualKey.F,
            ["G"] = VirtualKey.G,
            ["H"] = VirtualKey.H,
            ["I"] = VirtualKey.I,
            ["J"] = VirtualKey.J,
            ["K"] = VirtualKey.K,
            ["L"] = VirtualKey.L,
            ["M"] = VirtualKey.M,
            ["N"] = VirtualKey.N,
            ["O"] = VirtualKey.O,
            ["P"] = VirtualKey.P,
            ["Q"] = VirtualKey.Q,
            ["R"] = VirtualKey.R,
            ["S"] = VirtualKey.S,
            ["T"] = VirtualKey.T,
            ["U"] = VirtualKey.U,
            ["V"] = VirtualKey.V,
            ["W"] = VirtualKey.W,
            ["X"] = VirtualKey.X,
            ["Y"] = VirtualKey.Y,
            ["Z"] = VirtualKey.Z,
        };
        #endregion
        //
        #region PopulateItemHelp  =============================================
        void PopulateItemHelp(string input)
        {
            ItemHelp.Blocks.Clear();

            var strings = SplitOnNewLines(input);
            if (strings.Length == 0) return;

            foreach (var str in strings)
            {
                var run = new Run { Text = str };
                var para = new Paragraph();

                para.Inlines.Add(run);
                para.Margin = _spacing;

                ItemHelp.Blocks.Add(para);
            }
        }
        static readonly Thickness _spacing = new Thickness(0, 0, 0, 6);

        string[] SplitOnNewLines(string input)
        {
            var chars = input.ToCharArray();
            var output = new List<string>();
            var len = chars.Length;
            int j, i = 0;
            while (i < len)
            {
                if (chars[i] < ' ') { i++; continue; }

                for (j = i; j < len; j++)
                {
                    if (chars[j] >= ' ') continue;

                    output.Add(input.Substring(i, (j - i)));
                    i = j;
                    break;
                }
                if (i != j)
                {
                    output.Add(input.Substring(i, (len - i)));
                    break;
                }
            }
            return output.ToArray();
        }
        #endregion


        #region ItemName  =====================================================
        internal void ItemName_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            ItemIdentityTip.DataContext = obj.DataContext;
            ToolTipService.SetToolTip(obj, ItemIdentityTip);
        }
        internal void ItemName_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            //args.DragUI.SetContentFromDataPackage();
            var obj = sender as TextBlock;
            var mdl = obj.DataContext as LineModel;

            if (mdl.CanDrag)
            {
                mdl.DragStart(_dataRoot);
            }
            else
            {
                args.Cancel = true;
            }
        }
        internal void ItemName_DragOver(object sender, DragEventArgs e)
        {
            e.DragUIOverride.IsContentVisible = false;
            var obj = sender as TextBlock;
            var mdl = obj.DataContext as LineModel;

            var type = mdl.DragEnter(_dataRoot);
            switch (type)
            {
                case DropAction.None:
                    e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.None;
                    break;
                case DropAction.Move:
                    e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
                    break;
                case DropAction.Link:
                    e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Link;
                    break;
                case DropAction.Copy:
                    e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
                    break;
                default:
                    e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.None;
                    break;
            }
        }
        internal void ItemName_Drop(object sender, DragEventArgs e)
        {
            var obj = sender as TextBlock;
            var mdl = obj.DataContext as LineModel;
            mdl.DragDrop(_dataRoot);
        }
        #endregion

        #region ExpandLeft  ===================================================
        internal void TextBlockHightlight_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            obj.Opacity = 1.0;
        }
        internal void TextBlockHighlight_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            obj.Opacity = 0.5;
        }

        internal void RefreshExpandTree(LineModel model, TextBlock obj)
        {
            if (model.CanExpandLeft)
            {
                obj.Text = model.IsExpandedLeft ? LeftIsExtended : LeftCanExtend;
            }
            else
            {
                obj.Text = " ";
            }
        }
        internal void ExpandTree_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_selected == PointerModel(e))
            {
                var obj = sender as TextBlock;
                _selected = obj.DataContext as LineModel;
                _ = PostRefreshViewListAsync(ChangeType.ToggleLeft);
            }
        }
        #endregion

        #region ExpandRight  ==================================================
        internal void ExpandChoice_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_selected == PointerModel(e))
            {
                var obj = sender as TextBlock;
                _selected = obj.DataContext as LineModel;
                _ = PostRefreshViewListAsync(ChangeType.ToggleRight);
            }
        }
        #endregion

        #region ModelIdentity  ================================================
        internal void ModelIdentity_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            ModelIdentityTip.DataContext = obj.DataContext as LineModel;
            ToolTipService.SetToolTip(obj, ModelIdentityTip);
        }
        #endregion

        #region SortMode  =====================================================
        TextBlock _sortControl;
        internal void SortMode_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_selected == PointerModel(e))
            {
                var obj = sender as TextBlock;
                ExecuteSort(obj);
            }
        }
        void ExecuteSort(TextBlock obj)
        {
            if (obj == null)
            {
                return;
            }

            var mdl = obj.DataContext as LineModel;
            if (mdl.IsSortAscending)
            {
                mdl.IsSortAscending = false;
                mdl.IsSortDescending = true;
                obj.Text = SortDescending;
            }
            else if (mdl.IsSortDescending)
            {
                mdl.IsSortAscending = false;
                mdl.IsSortDescending = false;
                obj.Text = SortNone;
            }
            else
            {
                mdl.IsSortAscending = true;
                obj.Text = SortAscending;
            }

            _ = PostRefreshViewListAsync(ChangeType.FilterSortChanged);
        }
        #endregion

        #region UsageMode  ====================================================
        TextBlock _usageControl;
        internal void UsageMode_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_selected == PointerModel(e))
            {
                var obj = sender as TextBlock;
                ExecuteUsage(obj);
            }
        }
        void ExecuteUsage(TextBlock obj)
        {
            if (obj == null)
            {
                return;
            }

            var mdl = obj.DataContext as LineModel;
            if (mdl.IsUsedFilter)
            {
                mdl.IsUsedFilter = false;
                mdl.IsNotUsedFilter = true;
                obj.Text = UsageIsNotUsed;
            }
            else if (mdl.IsNotUsedFilter)
            {
                mdl.IsUsedFilter = false;
                mdl.IsNotUsedFilter = false;
                obj.Text = UsageAll;
            }
            else
            {
                mdl.IsUsedFilter = true;
                obj.Text = UsageIsUsed;
            }
            _ = PostRefreshViewListAsync(ChangeType.FilterSortChanged);
        }
        #endregion

        #region FilterMode  ===================================================
        TextBlock _filterControl;
        internal void FilterMode_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_selected == PointerModel(e))
            {
                var obj = sender as TextBlock;
                ExecuteFilterMode(obj);
            }
        }
        void ExecuteFilterMode(TextBlock obj)
        {
            if (obj == null) return;

            var mdl = obj.DataContext as LineModel;

            _ = PostRefreshViewListAsync(ChangeType.ToggleFilter);
        }
        #endregion

        #region FilterText  ===================================================
        internal void FilterText_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var obj = sender as TextBox;
            var mdl = obj.DataContext as LineModel;

            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Tab)
            {
                e.Handled = true;

                var txt = string.IsNullOrWhiteSpace(obj.Text) ? string.Empty : obj.Text;
                if (string.Compare(txt, (string)obj.Tag, true) == 0)
                {
                    SetDefaultFocus();
                    return;
                }

                obj.Tag = txt;
                PostSetFilterAsync(mdl, txt);
                mdl.IsExpandedLeft = true;
                
                _tryAfterRefresh = true;
                _ = PostRefreshViewListAsync(ChangeType.FilterSortChanged);
            }
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                e.Handled = true;

                mdl.IsFilterVisible = false;
                mdl.IsExpandedLeft = false;
                SetDefaultFocus();
                _ = PostRefreshViewListAsync(ChangeType.FilterSortChanged);
            }
        }
        #endregion

        #region TextProperty  =================================================
        internal void TextProperty_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = sender as TextBox;
            _focusControl = obj;
            _selected = obj.DataContext as LineModel;
            RefreshSelect(false);
        }
        internal void TextProperty_LostFocus(object sender, RoutedEventArgs e)
        {
            var obj = sender as TextBox;
            var mdl = obj.DataContext as PropertyModel;
            if ((string)obj.Tag != obj.Text)
            {
                mdl.PostSetValue(_dataRoot, obj.Text);
            }
        }
        internal void TextProperty_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Tab)
            {
                e.Handled = true;
                var obj = sender as TextBox;
                var mdl = obj.DataContext as PropertyModel;
                if ((string)obj.Tag != obj.Text)
                {
                    mdl.PostSetValue(_dataRoot, obj.Text);
                }                
                if (e.Key == Windows.System.VirtualKey.Enter)
                    FocusButton.Focus(FocusState.Keyboard);
                else
                    FindNextItemModel(mdl);
            }
            else if (e.Key == Windows.System.VirtualKey.Escape)
            {
                e.Handled = true;
                var obj = sender as TextBox;
                var mdl = obj.DataContext as PropertyModel;
                if ((string)obj.Tag != obj.Text)
                {
                    obj.Text = mdl.GetTextValue(_dataRoot) ?? string.Empty;
                }
                SetDefaultFocus();
            }
        }
        #endregion

        #region CheckProperty  ================================================
       internal void CheckProperty_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = sender as CheckBox;
            _focusControl = obj;
            _selected = obj.DataContext as LineModel;
            RefreshSelect(false);
        }
        internal void Check_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var obj = sender as CheckBox;
            var mdl = obj.DataContext as PropertyModel;
            var val = obj.IsChecked ?? false;

            if (e.Key == VirtualKey.Escape)
            {
                e.Handled = true;
                SetDefaultFocus();
            }
            else if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;
                _ignoreNextCheckBoxEvent = true;
                mdl.PostSetValue(_dataRoot, !val);
            }
            else if (e.Key == Windows.System.VirtualKey.Tab)
            {
                e.Handled = true;
                FindNextItemModel(mdl);
            }
        }
        bool _ignoreNextCheckBoxEvent;
        internal void CheckProperty_Checked(object sender, RoutedEventArgs e)
        {
            if (_ignoreNextCheckBoxEvent)
            {
                _ignoreNextCheckBoxEvent = false;
            }
            else
            {
                var obj = sender as CheckBox;
                var mdl = obj.DataContext as PropertyModel;
                var val = obj.IsChecked ?? false;
                mdl.PostSetValue(_dataRoot, val);
            }
        }
        #endregion

        #region ComboProperty  ================================================
        internal void ComboProperty_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = sender as ComboBox;
            _focusControl = obj;
            _selected = obj.DataContext as LineModel;
            RefreshSelect(false);
        }
        internal void ComboProperty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as ComboBox;
            var mdl = obj.DataContext as PropertyModel;
            //mdl.PostSetValue(_chef, obj.SelectedIndex);
        }
        internal void ComboProperty_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var obj = sender as ComboBox;
            var mdl = obj.DataContext as LineModel;
            if (e.Key == VirtualKey.Escape)
            {
                e.Handled = true;
                SetDefaultFocus();
            }
            else if (e.Key == VirtualKey.Tab)
            {
                e.Handled = true;
                FindNextItemModel(mdl);
            }
        }
        #endregion

        #region PropertyBorder  ===============================================
        internal void PropertyBorder_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as Border;
            ItemIdentityTip.DataContext = obj.DataContext as LineModel;
        }
        #endregion

        #region FindNextItemModel  ============================================
        void FindNextItemModel(LineModel m)
        {
            var k = _viewList.IndexOf(m) + 1;
            for (int i = k; i < _viewList.Count; i++)
            {
                var mdl = _viewList[i];
                if (!(mdl is PropertyModel)) continue;
                TrySetControlFocus(mdl);
                return;
            }
            SetDefaultFocus();
        }
        #endregion

    }
}
