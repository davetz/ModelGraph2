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

namespace ModelGraph.Controls
{
    public sealed partial class ModelTreeControl : Page, IPageControl, IModelPageControl
    {
        public ModelTreeControl(TreeModel root)
        {             
            _treeRoot = root;
            _dataRoot = root.DataRoot;

            InitializeComponent();
            Initialize();
            Loaded += ModelTreeControl_Loaded;
        }

        private void ModelTreeControl_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        #region PostRefreshViewList  ==========================================
        private async System.Threading.Tasks.Task PostRefreshViewListAsync(ChangeType change = ChangeType.None)
        {
            LineModel leading = (_viewList is null || _viewList.Count == 0) ? null : _viewList[0];
            ResetCacheDelta(_selected);
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { _treeRoot.RefreshViewList(ViewSize, leading, _selected, change); });
            Refresh();
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
        int ViewSize => (int)(Height / _elementHieght);
        #endregion

        #region Fields  =======================================================
        Root _dataRoot;
        TreeModel _treeRoot;
        LineModel _selected;
        List<LineModel> _viewList = new List<LineModel>();
        List<LineCommand> _menuCommands = new List<LineCommand>();
        List<LineCommand> _buttonCommands = new List<LineCommand>();

        ToolTip _itemIdentityTip;
        ToolTip _modelIdentityTip;

        int _levelIndent;
        int _elementHieght;

        Style _expanderStyle;
        Style _itemKindStyle;
        Style _itemNameStyle;
        Style _itemInfoStyle;
        Style _sortModeStyle;
        Style _usageModeStyle;
        Style _totalCountStyle;
        Style _indentTreeStyle;
        Style _filterModeStyle;
        Style _filterTextStyle;
        Style _filterCountStyle;
        Style _itemHasErrorStyle;
        Style _propertyNameStyle;
        Style _textPropertyStyle;
        Style _checkPropertyStyle;
        Style _comboPropertyStyle;
        Style _modelIdentityStyle;
        Style _propertyBorderStyle;

        ToolTip[] _menuItemTips;
        ToolTip[] _itemButtonTips;

        Button[] _itemButtons;
        MenuFlyoutItem[] _menuItems;
        int _menuItemsCount;

        int Count => (_viewList == null) ? 0 : _viewList.Count;

        // segoe ui symbol font glyphs  =====================
        const string _fontFamily = "Segoe MDL2 Assets";
        const string _leftCanExtend = "\u25b7";
        const string _leftIsExtended = "\u25e2";

        const string _rightCanExtend = "\u25c1";
        const string _rightIsExtended = "\u25e3";

        const string _sortNone = "\u2012";
        const string _sortAscending = "\u2228";
        const string _sortDescending = "\u2227";

        const string _usageAll = "A";
        const string _usageIsUsed = "U";
        const string _usageIsNotUsed = "N";

        const string _filterCanShow = "\uE71C";
        const string _filterIsShowing = "\uE71C\uEBE7";

        const string _itemHasErrorText = "\uE783";

        string _sortModeTip;
        string _usageModeTip;
        string _leftExpandTip;
        string _totalCountTip;
        string _filterTextTip;
        string _filterCountTip;
        string _rightExpandTip;
        string _filterExpandTip;
        string _itemHasErrorTip;

        // position all unused cache elements offScreen
        const int notVisible = 32767;
        #endregion

        #region IPageControl  =================================================
        public void CreateNewPage(IModel model, ControlType ctlType)
        {
            if (model is null) return;
            _ = ModelPageService.Current.CreateNewPageAsync(model, ctlType);
        }
        public IModel IModel => _treeRoot;
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

            _itemIdentityTip = new ToolTip();
            _itemIdentityTip.Opened += ItemIdentityTip_Opened;

            _modelIdentityTip = new ToolTip();
            _modelIdentityTip.Opened += ModelIdentityTip_Opened;

            _levelIndent = (int)(Resources["LevelIndent"] as Double?).Value;
            _elementHieght = (int)(Resources["ElementHieght"] as Double?).Value;

            _expanderStyle = Resources["ExpanderStyle"] as Style;
            _itemKindStyle = Resources["ItemKindStyle"] as Style;
            _itemNameStyle = Resources["ItemNameStyle"] as Style;
            _itemInfoStyle = Resources["ItemInfoStyle"] as Style;
            _sortModeStyle = Resources["SortModeStyle"] as Style;
            _usageModeStyle = Resources["UsageModeStyle"] as Style;
            _totalCountStyle = Resources["TotalCountStyle"] as Style;
            _indentTreeStyle = Resources["IndentTreeStyle"] as Style;
            _filterModeStyle = Resources["FilterModeStyle"] as Style;
            _filterTextStyle = Resources["FilterTextStyle"] as Style;
            _filterCountStyle = Resources["FilterCountStyle"] as Style;
            _itemHasErrorStyle = Resources["ItemHasErrorStyle"] as Style;
            _propertyNameStyle = Resources["PropertyNameStyle"] as Style;
            _textPropertyStyle = Resources["TextPropertyStyle"] as Style;
            _checkPropertyStyle = Resources["CheckPropertyStyle"] as Style;
            _comboPropertyStyle = Resources["ComboPropertyStyle"] as Style;
            _modelIdentityStyle = Resources["ModelIdentityStyle"] as Style;
            _propertyBorderStyle = Resources["PropertyBorderStyle"] as Style;

            _sortModeTip = "010S".GetLocalized();
            _usageModeTip = "00ES".GetLocalized();
            _leftExpandTip = "006S".GetLocalized();
            _totalCountTip = "007S".GetLocalized();
            _filterTextTip = "008S".GetLocalized();
            _filterCountTip = "009S".GetLocalized();
            _rightExpandTip = "00AS".GetLocalized();
            _filterExpandTip = "00BS".GetLocalized();
            _itemHasErrorTip = "00FS".GetLocalized();

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

            _itemIdentityTip.Opened -= ItemIdentityTip_Opened;
            _modelIdentityTip.Opened -= ModelIdentityTip_Opened;

            _treeRoot = null;
            _selected = null;
            _viewList.Clear();
            _menuCommands.Clear();
            _buttonCommands.Clear();
            _itemIdentityTip = null;
            _modelIdentityTip = null;
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
            var i = (int)(p.Position.Y / _elementHieght);
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

            ValidateCache(N);

            for (int i = 0; i < N; i++)
            {
                AddStackPanel(i, _viewList[i]);
            }

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

            var cacheIndex = TryGetCacheIndex();
            if (cacheIndex < 0) return;

            if (_sortModeCache[cacheIndex] != null && _sortModeCache[cacheIndex].DataContext != null)
            {
                _sortControl = _sortModeCache[cacheIndex];
                var acc = new KeyboardAccelerator { Key = VirtualKey.S, Modifiers = VirtualKeyModifiers.Control};
                acc.Invoked += Accelerator_SortMode_Invoked;
                TreeCanvas.KeyboardAccelerators.Add(acc);
            }

            if (_usageModeCache[cacheIndex] != null && _usageModeCache[cacheIndex].DataContext != null)
            {
                _usageControl = _usageModeCache[cacheIndex];
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

            if (_filterModeCache[cacheIndex] != null && _filterModeCache[cacheIndex].DataContext != null)
            {
                _filterControl = _filterModeCache[cacheIndex];
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

        #region RefreshHelpers  ===============================================
        //
        #region TryGetCacheIndex  =============================================
        int TryGetCacheIndex()
        {
            if (Count < 0) return -1;

            var viewIndex = _viewList.IndexOf(_selected);
            if (viewIndex < 0)
            {
                _selected = _viewList[0];
                viewIndex = 0;
            }
           // _root.SelectModel = _selectModel;
            var cacheIndex = _cacheIndex[viewIndex];

            SelectGrid.Width = ActualWidth;
            Canvas.SetTop(SelectGrid, (viewIndex * _elementHieght));

            return cacheIndex;
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

            var cacheIndex = TryGetCacheIndex();
            if (cacheIndex < 0) return;

            if (_selected.CanFilter)
            {
                if (_selected.IsFilterVisible)
                {
                    _selected.IsFilterFocus = false;
                    SetFocus(_filterTextCache[cacheIndex]);
                }
                else
                {
                    _tryAfterRefresh = true;
                    _ = PostRefreshViewListAsync(ChangeType.ToggleFilter);
                }
            }
            else if (_selected is PropertyModel pm)
            {
                if (pm.IsTextModel) SetFocus(_textPropertyCache[cacheIndex]);
                else if (pm.IsCheckModel) SetFocus(_checkPropertyCache[cacheIndex]);
                else if (pm.IsComboModel) SetFocus(_comboPropertyCache[cacheIndex]);
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
        //
        #endregion


        #region ItemName  =====================================================
        void ItemName_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            _itemIdentityTip.DataContext = obj.DataContext;
            ToolTipService.SetToolTip(obj, _itemIdentityTip);
        }
        void ItemName_DragStarting(UIElement sender, DragStartingEventArgs args)
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
        void ItemName_DragOver(object sender, DragEventArgs e)
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
        void ItemName_Drop(object sender, DragEventArgs e)
        {
            var obj = sender as TextBlock;
            var mdl = obj.DataContext as LineModel;
            mdl.DragDrop(_dataRoot);
        }
        #endregion

        #region ExpandLeft  ===================================================
        void TextBlockHightlight_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            obj.Opacity = 1.0;
        }
        void TextBlockHighlight_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            obj.Opacity = 0.5;
        }

        void RefreshExpandTree(LineModel model, TextBlock obj)
        {
            if (model.CanExpandLeft)
            {
                obj.Text = model.IsExpandedLeft ? _leftIsExtended : _leftCanExtend;
            }
            else
            {
                obj.Text = " ";
            }
        }
        void ExpandTree_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
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
        void ExpandChoice_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
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
        void ModelIdentity_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            _modelIdentityTip.DataContext = obj.DataContext as LineModel;
            ToolTipService.SetToolTip(obj, _modelIdentityTip);
        }
        #endregion

        #region SortMode  =====================================================
        TextBlock _sortControl;
        void SortMode_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
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
                obj.Text = _sortDescending;
            }
            else if (mdl.IsSortDescending)
            {
                mdl.IsSortAscending = false;
                mdl.IsSortDescending = false;
                obj.Text = _sortNone;
            }
            else
            {
                mdl.IsSortAscending = true;
                obj.Text = _sortAscending;
            }

            _ = PostRefreshViewListAsync(ChangeType.FilterSortChanged);
        }
        #endregion

        #region UsageMode  ====================================================
        TextBlock _usageControl;
        void UsageMode_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
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
                obj.Text = _usageIsNotUsed;
            }
            else if (mdl.IsNotUsedFilter)
            {
                mdl.IsUsedFilter = false;
                mdl.IsNotUsedFilter = false;
                obj.Text = _usageAll;
            }
            else
            {
                mdl.IsUsedFilter = true;
                obj.Text = _usageIsUsed;
            }
            _ = PostRefreshViewListAsync(ChangeType.FilterSortChanged);
        }
        #endregion

        #region FilterMode  ===================================================
        TextBlock _filterControl;
        void FilterMode_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
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
        void FilterText_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
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
                _treeRoot.SetFilterText(mdl, txt);
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
        private void TextProperty_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = sender as TextBox;
            _focusControl = obj;
            _selected = obj.DataContext as LineModel;
            RefreshSelect(false);
        }
        void TextProperty_LostFocus(object sender, RoutedEventArgs e)
        {
            var obj = sender as TextBox;
            var mdl = obj.DataContext as PropertyModel;
            if ((string)obj.Tag != obj.Text)
            {
                mdl.PostSetValue(_dataRoot, obj.Text);
            }
        }
        void TextProperty_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
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
        private void CheckProperty_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = sender as CheckBox;
            _focusControl = obj;
            _selected = obj.DataContext as LineModel;
            RefreshSelect(false);
        }
        void Check_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
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
        void CheckProperty_Checked(object sender, RoutedEventArgs e)
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
        private void ComboProperty_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = sender as ComboBox;
            _focusControl = obj;
            _selected = obj.DataContext as LineModel;
            RefreshSelect(false);
        }
        void ComboProperty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as ComboBox;
            var mdl = obj.DataContext as PropertyModel;
            //mdl.PostSetValue(_chef, obj.SelectedIndex);
        }
        void ComboProperty_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
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
        void PropertyBorder_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as Border;
            _itemIdentityTip.DataContext = obj.DataContext as LineModel;
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
