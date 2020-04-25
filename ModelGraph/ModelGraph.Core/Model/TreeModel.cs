using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Core
{
    /// <summary>Flat list of LineModel that emulates a UI tree view</summary>
    public class TreeModel : StoreOf<LineModel>, IModel
    {
        public Chef DataChef => Owner as Chef;
        public Item RootItem { get; internal set; }
        public IRootModel RootModel { get; protected set; }
        public IPageControl PageControl { get; set; } // reference the UI PageControl       
        public ControlType ControlType { get; private set; }
        public string TitleName => "There isn't any Title Name yet";
        public string TitleSummary => "There isn't any Title Summary yet";

        private List<(int index, string filter)> _filter = new List<(int index, string filter)>(20);
        private List<(Item item, IdKey MId, byte depth, byte delta)> _expansion = new List<(Item item, IdKey MId, byte depth, byte delta)>(100);

        #region Constructor  ==================================================
        internal TreeModel(Chef chef) // invoked within RootTreeModel constructor
        {
            Owner = chef;
            RootItem = chef;
            ControlType = ControlType.PrimaryTree;

            chef.Add(this);
        }
        internal TreeModel(Chef chef, RootTreeModel rootModel, Item rootItem) // created by the TreeRootModel
        {
            Owner = chef;
            RootItem = rootItem;
            RootModel = rootModel;
            ControlType = ControlType.PartialTree;

            chef.Add(this);
        }
        #endregion

        #region IModel  =======================================================
        public void Release()
        {
            if (Owner is null) return;

            Items.Clear();
            _filter.Clear();
            _expansion.Clear();
            DataChef.Remove(this);

            if (this == RootModel) DataChef.Release();

            Owner = null;
        }
        #endregion
    }
}
