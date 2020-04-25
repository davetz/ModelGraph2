﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using ModelGraph.Core;
using ModelGraph.Helpers;
using Windows.Storage.Pickers;
using Windows.Storage;
using ModelGraph.Services;
using System.Diagnostics;
using Windows.System;
using ModelGraph.Repository;

namespace ModelGraph.Controls
{
    public sealed partial class ModelTreeControl : Page, IPageControl, IModelPageControl
    {
        //public Grid PrevOwner { get; set; }

        public ModelTreeControl(IModel root)
        {
            _root = root;

            InitializeComponent();
            Initialize();
        }

        #region SetSize  ======================================================
        public void SetSize(double width, double height)
        {
            if (_root is null || TreeCanvas is null) return;
            if (height > 0)
            {
                TreeCanvas.Width = Width = width;
                TreeCanvas.Height = Height = height;
                //_root.ViewCapacity = (int)(Height / _elementHieght);
                _viewIsReady = true;
                //_root.PostRefreshViewList(_selectModel);
            }
        }
        bool ViewIsNotReady() => !_viewIsReady;
        bool _viewIsReady;

        #endregion

        #region Fields  =======================================================
        IModel _root;
        ItemModel _selectModel;
        List<ItemModel> _viewList = new List<ItemModel>();
        List<ModelCommand> _menuCommands = new List<ModelCommand>();
        List<ModelCommand> _buttonCommands = new List<ModelCommand>();

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
        public IModel IModel => _root;
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

            RefreshRoot();
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

            foreach (var cmd in _menuCommands) { cmd.Release(); }
            foreach (var cmd in _buttonCommands) { cmd.Release(); }

            _root = null;
            _selectModel = null;
            _viewList = null;
            _menuCommands.Clear();
            _menuCommands = null;
            _buttonCommands.Clear();
            _buttonCommands = null;
            _itemIdentityTip = null;
            _modelIdentityTip = null;
        }
        #endregion

        #region PostRefreshViewList  ==========================================
        void PostRefreshViewList(ItemModel m, int s = 0, ChangeType c = ChangeType.NoChange)
        {
            ResetCacheDelta(m);
            //_root.PostRefreshViewList(m, s, c);
        }
        #endregion

        #region KeyboardAccelerators  =========================================
        private void KeyPageUp_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            ChangeScroll(2 - Count);
            args.Handled = true;
        }
        private void KeyPageDown_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            ChangeScroll(Count - 2);
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
            PostRefreshViewList(_selectModel, 0, ChangeType.GoToEnd);
            args.Handled = true;
        }
        private void KeyHome_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            PostRefreshViewList(_selectModel, 0, ChangeType.GoToHome);
            args.Handled = true;
        }

        private void KeyLeft_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_selectModel.CanExpandLeft)
            {
                PostRefreshViewList(_selectModel, 0, ChangeType.ToggleLeft);
            }
            args.Handled = true;
        }
        private void KeyRight_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_selectModel.CanExpandRight)
            {
                PostRefreshViewList(_selectModel, 0, ChangeType.ToggleRight);
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
            if (_selectModel != null)
            {
                _selectModel.IsFilterVisible = false;
                _selectModel.IsExpandedLeft = false;
                PostRefreshViewList(_selectModel, 0, ChangeType.FilterSortChanged);
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
            if (_selectModel is null)
            {
                if (_viewList is null || _viewList.Count == 0) return;
                _selectModel = _viewList[0];
                RefreshSelect();
            }
            else
            {
                var i = _viewList.IndexOf(_selectModel) - 1;
                if (i >= 0 && i < _viewList.Count)
                {
                    _selectModel = _viewList[i];
                    RefreshSelect();
                }
                else
                    ChangeScroll(-1);
            }
        }
        void TryGetNextModel()
        {
            SetDefaultFocus();
            if (_selectModel is null)
            {
                if (_viewList is null || _viewList.Count == 0) return;
                _selectModel = _viewList[0];
                RefreshSelect();
            }
            else
            {
                var i = _viewList.IndexOf(_selectModel) + 1;
                if (i > 0 && i < _viewList.Count)
                {
                    _selectModel = _viewList[i];
                    RefreshSelect();
                }
                else
                    ChangeScroll(1);
            }
        }
        #endregion

        #region AppButton_Click  ==============================================
        private void AppButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var cmd = btn.Tag as ModelCommand;
            cmd.Execute();
        }
        #endregion

        #region Button_Click  =================================================
        void ItemButton_Click(object sender, RoutedEventArgs e)
        {
            var obj = sender as Button;
            var cmd = obj.DataContext as ModelCommand;
            cmd.Execute();
        }
        void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var obj = sender as MenuFlyoutItem;
            var cmd = obj.DataContext as ModelCommand;
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
                var delta = (cp.Properties.MouseWheelDelta < 0) ? 3 : -3;
                ChangeScroll(delta);
            }
        }
        void ChangeScroll(int delta)
        {
            PostRefreshViewList(_selectModel, delta);
        }
        #endregion

        #region ToolTip_Opened  ===============================================
        void ItemIdentityTip_Opened(object sender, RoutedEventArgs e)
        {
            var tip = sender as ToolTip;
            var mdl = tip.DataContext as ItemModel;
            var content = mdl.ModelSummary;
            tip.Content = string.IsNullOrWhiteSpace(content) ? null : content;
        }
        void ModelIdentityTip_Opened(object sender, RoutedEventArgs e)
        {
            var tip = sender as ToolTip;
            var mdl = tip.DataContext as ItemModel;
            tip.Content = mdl.ModelIdentity;
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
            _selectModel = PointerModel(e);
            SetDefaultFocus();
            RefreshSelect();
            e.Handled = true;
        }
        ItemModel PointerModel(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var p = e.GetCurrentPoint(TreeCanvas);
            var i = (int)(p.Position.Y / _elementHieght);
            return (Count == 0) ? null : (i < 0) ? _viewList[0] : (i < Count) ? _viewList[i] : _viewList[Count - 1];
        }
        #endregion


        #region Refresh  ======================================================
        public void Refresh()
        {
            if (ViewIsNotReady()) return;
            if (_viewList is null) return;
            //if (_root.IsChanged) RefreshRoot();

            _viewList.Clear();
            //_viewList.AddRange(_root.ViewFlatList);
           // _selectModel = _root.SelectModel;

            _pointWheelEnabled = false;

            var N = _viewList.Count;
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
            var buttonCommands = new List<ModelCommand>();
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
                        btn.Content = cmd.Name;
                        btn.Visibility = Visibility.Visible;
                        ToolTipService.SetToolTip(btn, cmd.Summary);
                    }
                    else
                    {
                        btn.Visibility = Visibility.Collapsed;
                    }
                }
            }
            ModelTitle.Text = _root.TitleName;
           // _root.IsChanged = false;
        }

        #endregion

        #region RefreshSelect  ================================================
        void RefreshSelect(bool restoreFocus = true)
        {
            TreeCanvas.KeyboardAccelerators.Clear();
            _acceleratorKeyCommands.Clear();

            _sortControl = _filterControl = null;

            if (Count == 0 || _selectModel == null)
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

            if (_selectModel.CanDrag)
            {
                var acc = new KeyboardAccelerator { Key = VirtualKey.C, Modifiers = VirtualKeyModifiers.Control };
                acc.Invoked += Accelerator_ModelCopy_Invoked;
                TreeCanvas.KeyboardAccelerators.Add(acc);
            }

            if (_selectModel.DragEnter() != DropAction.None)
            {
                var acc = new KeyboardAccelerator { Key = VirtualKey.V, Modifiers = VirtualKeyModifiers.Control };
                acc.Invoked += Accelerator_ModelPaste_Invoked;
                TreeCanvas.KeyboardAccelerators.Add(acc);
            }

            if (_filterModeCache[cacheIndex] != null && _filterModeCache[cacheIndex].DataContext != null)
            {
                _filterControl = _filterModeCache[cacheIndex];
            }


            if (_selectModel.ModelDescription != null)
            {
                HelpButton.Visibility = Visibility.Visible;
                PopulateItemHelp(_selectModel.ModelDescription);
            }
            else
            {
                HelpButton.Visibility = Visibility.Collapsed;
            }

            _selectModel.MenuComands(_menuCommands);
            _selectModel.PageButtonComands(_buttonCommands);

            var cmds = _buttonCommands;
            var len1 = cmds.Count;
            var len2 = _itemButtons.Length;

            for (int i = 0; i < len2; i++)
            {
                if (i < len1)
                {
                    var cmd = _buttonCommands[i];
                    _itemButtons[i].DataContext = cmd;
                    _itemButtons[i].Content = cmd.Name;
                    _itemButtonTips[i].Content = cmd.Summary;
                    _itemButtons[i].Visibility = Visibility.Visible;
                    var key = cmd.AcceleratorKey;
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
                        _menuItems[i].Text = cmd.Name;
                        _menuItemTips[i].Content = cmd.Summary;
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

            var viewIndex = _viewList.IndexOf(_selectModel);
            if (viewIndex < 0)
            {
                _selectModel = _viewList[0];
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
        private void TrySetControlFocus(ItemModel focusModel = null)
        {
            if (focusModel != null) _selectModel = focusModel;

            var cacheIndex = TryGetCacheIndex();
            if (cacheIndex < 0) return;

            if (_selectModel.CanFilter)
            {
                if (_selectModel.IsFilterVisible)
                {
                    _selectModel.IsFilterFocus = false;
                    SetFocus(_filterTextCache[cacheIndex]);
                }
                else
                {
                    _tryAfterRefresh = true;
                    PostRefreshViewList(_selectModel, 0, ChangeType.ToggleFilter);
                }
            }
            else if (_selectModel.IsProperty)
            {
                if (_selectModel.IsTextProperty) SetFocus(_textPropertyCache[cacheIndex]);
                else if (_selectModel.IsCheckProperty) SetFocus(_checkPropertyCache[cacheIndex]);
                else if (_selectModel.IsComboProperty) SetFocus(_comboPropertyCache[cacheIndex]);
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
            if (_selectModel.CanDrag)
                _selectModel.DragStart();
            args.Handled = true;
        }
        private void Accelerator_ModelPaste_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            if (_selectModel.DragEnter() != DropAction.None)
            _selectModel.DragDrop();
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
            if (_acceleratorKeyCommands.TryGetValue(sender, out ModelCommand cmd)) cmd.Execute();
            args.Handled = true;
        }
        private readonly Dictionary<KeyboardAccelerator, ModelCommand> _acceleratorKeyCommands = new Dictionary<KeyboardAccelerator, ModelCommand>();

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
            var mdl = obj.DataContext as ItemModel;

            if (mdl.CanDrag)
            {
                mdl.DragStart();
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
            var mdl = obj.DataContext as ItemModel;

            var type = mdl.DragEnter();
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
            var mdl = obj.DataContext as ItemModel;
            mdl.DragDrop();
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

        void RefreshExpandTree(ItemModel model, TextBlock obj)
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
            if (_selectModel == PointerModel(e))
            {
                var obj = sender as TextBlock;
                _selectModel = obj.DataContext as ItemModel;
                PostRefreshViewList(_selectModel, 0, ChangeType.ToggleLeft);
            }
        }
        #endregion

        #region ExpandRight  ==================================================
        void ExpandChoice_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_selectModel == PointerModel(e))
            {
                var obj = sender as TextBlock;
                _selectModel = obj.DataContext as ItemModel;
                PostRefreshViewList(_selectModel, 0, ChangeType.ToggleRight);
            }
        }
        #endregion

        #region ModelIdentity  ================================================
        void ModelIdentity_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var obj = sender as TextBlock;
            _modelIdentityTip.DataContext = obj.DataContext as ItemModel;
            ToolTipService.SetToolTip(obj, _modelIdentityTip);
        }
        #endregion

        #region SortMode  =====================================================
        TextBlock _sortControl;
        void SortMode_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_selectModel == PointerModel(e))
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

            var mdl = obj.DataContext as ItemModel;
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

            PostRefreshViewList(_selectModel, 0, ChangeType.FilterSortChanged);
        }
        #endregion

        #region UsageMode  ====================================================
        TextBlock _usageControl;
        void UsageMode_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_selectModel == PointerModel(e))
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

            var mdl = obj.DataContext as ItemModel;
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
            PostRefreshViewList(_selectModel, 0, ChangeType.FilterSortChanged);
        }
        #endregion

        #region FilterMode  ===================================================
        TextBlock _filterControl;
        void FilterMode_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_selectModel == PointerModel(e))
            {
                var obj = sender as TextBlock;
                ExecuteFilterMode(obj);
            }
        }
        void ExecuteFilterMode(TextBlock obj)
        {
            if (obj == null) return;

            var mdl = obj.DataContext as ItemModel;

            PostRefreshViewList(_selectModel, 0, ChangeType.ToggleFilter);
        }
        #endregion

        #region FilterText  ===================================================
        void FilterText_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var obj = sender as TextBox;
            var mdl = obj.DataContext as ItemModel;

            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Tab)
            {
                e.Handled = true;

                var str = string.IsNullOrWhiteSpace(obj.Text) ? string.Empty : obj.Text;
                if (string.Compare(str, (string)obj.Tag, true) == 0)
                {
                    SetDefaultFocus();
                    return;
                }

                obj.Tag = str;
                mdl.UpdateViewFilter(str);
                mdl.IsExpandedLeft = true;
                
                _tryAfterRefresh = true;
                PostRefreshViewList(_selectModel, 0, ChangeType.FilterSortChanged);
            }
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                e.Handled = true;

                mdl.ViewFilter = null;
                mdl.IsFilterVisible = false;
                mdl.IsExpandedLeft = false;
                SetDefaultFocus();
                PostRefreshViewList(_selectModel, 0, ChangeType.FilterSortChanged);
            }
        }
        #endregion

        #region TextProperty  =================================================
        private void TextProperty_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = sender as TextBox;
            _focusControl = obj;
            _selectModel = obj.DataContext as ItemModel;
            RefreshSelect(false);
        }
        void TextProperty_LostFocus(object sender, RoutedEventArgs e)
        {
            var obj = sender as TextBox;
            var mdl = obj.DataContext as ItemModel;
            if ((string)obj.Tag != obj.Text)
            {
                mdl.PostSetValue(obj.Text);
            }
        }
        void TextProperty_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Tab)
            {
                e.Handled = true;
                var obj = sender as TextBox;
                var mdl = obj.DataContext as ItemModel;
                if ((string)obj.Tag != obj.Text)
                {
                    mdl.PostSetValue(obj.Text);
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
                var mdl = obj.DataContext as ItemModel;
                if ((string)obj.Tag != obj.Text)
                {
                    obj.Text = mdl.TextValue ?? string.Empty;
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
            _selectModel = obj.DataContext as ItemModel;
            RefreshSelect(false);
        }
        void Check_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var obj = sender as CheckBox;
            var mdl = obj.DataContext as ItemModel;
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
                mdl.PostSetValue(!val);
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
                var mdl = obj.DataContext as ItemModel;
                var val = obj.IsChecked ?? false;
                mdl.PostSetValue(val);
            }
        }
        #endregion

        #region ComboProperty  ================================================
        private void ComboProperty_GotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var obj = sender as ComboBox;
            _focusControl = obj;
            _selectModel = obj.DataContext as ItemModel;
            RefreshSelect(false);
        }
        void ComboProperty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as ComboBox;
            var mdl = obj.DataContext as ItemModel;
            mdl.PostSetValue(obj.SelectedIndex);
        }
        void ComboProperty_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var obj = sender as ComboBox;
            var mdl = obj.DataContext as ItemModel;
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
            _itemIdentityTip.DataContext = obj.DataContext as ItemModel;
        }
        #endregion

        #region FindNextItemModel  ============================================
        void FindNextItemModel(ItemModel m)
        {
            var k = _viewList.IndexOf(m) + 1;
            for (int i = k; i < _viewList.Count; i++)
            {
                var mdl = _viewList[i];
                if (!mdl.IsProperty) continue;
                TrySetControlFocus(mdl);
                return;
            }
            SetDefaultFocus();
        }
        #endregion

    }
}
