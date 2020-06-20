
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ModelGraph.Core
{
    internal class FilterSort
    {
        private int _count;
        private Usage _usage = Usage.None;
        private Sorting _sorting = Sorting.Unsorted;
        private byte _delta;
        private bool _sortChanged;
        private bool _filterChanged;
        private string _filterText;
        private Regex _filter;
        private List<(int I, bool IN, string TX)> _selector;


        public FilterSort(LineModel model, string filterText)
        {
            SetFilter(model, filterText);
        }

        #region Parms  ========================================================
        internal int Count => _count;
        internal List<(int I, bool IN, string TX)> Selector => _selector is null ? new List<(int I, bool IN, string TX)>(0) : _selector;
        internal string FilterString => _filterText is null ? string.Empty : _filterText;
        internal (Sorting, Usage, string) Parms => (_sorting, _usage, _filterText is null ? string.Empty : _filterText);

        internal bool SetUsage(LineModel model, Usage usage) { _filterChanged |= _usage != usage; _usage = usage; return Check(model); }
        internal bool SetSorting(LineModel model, Sorting sorting) { _sortChanged = _sorting != sorting; _sorting = sorting; return Check(model); }
        internal bool SetFilter(LineModel model, string filterText)
        {
            var noFilter = string.IsNullOrWhiteSpace(filterText);
            if (noFilter)
            {
                _filterChanged |= !(_filterText is null);
                _filterText = null;
                _filter = null;
            }
            else if (_filter is null || _filterText is null || !IsSame(_filterText, filterText))
            {
                _filterChanged = true;
                _filterText = filterText;
                _filter = filterText.Contains("*") ?
                    new Regex(filterText, RegexOptions.Compiled | RegexOptions.IgnoreCase) :
                    new Regex($".*{filterText}.*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
            return Check(model);
        }
        internal bool ClearFilter(LineModel model) { return SetFilter(model, null); }

        private bool IsSame(string a, string b)
        {
            return N(a) ? N(b) : (!N(b) && E(a, b));

            bool N(string v) => string.IsNullOrWhiteSpace(v); //is NULL or blank
            bool E(string p, string q) => (string.Compare(p, q) == 0); //are EQUAL
        }

        private bool Check(LineModel model)
        {
            var doRefresh = _usage != Usage.None || _sorting != Sorting.Unsorted || !string.IsNullOrWhiteSpace(_filterText);
            if (doRefresh) Refresh(model);
            return !doRefresh;
        }
        #endregion

        #region Refresh  ======================================================
        internal void Refresh(LineModel model)
        {
            var root = model.DataRoot;
            if (_delta != model.ChildDelta || _selector is null || _selector.Count != model.Count)
            {
                #region need to build to new selector
                _delta = model.ChildDelta;
                _filterChanged = true;
                _selector = new List<(int I, bool IN, string ID)>(model.Count);
                for (int i = 0; i < model.Count; i++)
                {
                    _selector.Add((i, true, model.Items[i].GetFilterSortId(root)));
                }
                #endregion
            }

            if (_filterChanged)
            {
                #region need to revalidate the selector
                _count = 0;
                if (_filterText is null && _usage != Usage.None)
                {
                    for (int i = 0; i < _selector.Count; i++)
                    {
                        var (I, IN, TX) = _selector[i];
                        var child = model.Items[I];
                        var used = child.IsItemUsed;
                        var tIN = used ? (_usage == Usage.IsUsed) : (_usage == Usage.IsNotUsed);
                        if (tIN) _count++;
                        if (tIN != IN) _selector[i] = (I, tIN, TX);
                    }
                }
                else if (!(_filterText is null) && _usage == Usage.None)
                {
                    for (int i = 0; i < _selector.Count; i++)
                    {
                        var (I, IN, TX) = _selector[i];
                        var tIN = _filter.IsMatch(TX);
                        if (tIN) _count++;
                        if (tIN != IN) _selector[i] = (I, tIN, TX);
                    }
                }
                else if (!(_filterText is null) && _usage != Usage.None)
                {
                    for (int i = 0; i < _selector.Count; i++)
                    {
                        var (I, IN, TX) = _selector[i];
                        var child = model.Items[I];
                        var used = child.IsItemUsed;
                        var tIN = used ? (_usage == Usage.IsUsed) : (_usage == Usage.IsNotUsed);
                        tIN |= _filter.IsMatch(TX);
                        if (tIN) _count++;
                        if (tIN != IN) _selector[i] = (I, tIN, TX);
                    }
                }
                else
                {
                    for (int i = 0; i < _selector.Count; i++)
                    {
                        var (I, IN, TX) = _selector[i];
                        _selector[i] = (I, true, TX);
                    }
                    _count = _selector.Count;
                }
                #endregion
            }

            if (_sortChanged)
            {
                #region need to resort the selector
                _sortChanged = false;
                _selector.Sort(CompareSelector);
                #endregion
            }
        }
        private static int CompareSelector((int, bool, string) a, (int, bool, string) b) => a.Item3.CompareTo(b.Item3);
        #endregion
    }
}
