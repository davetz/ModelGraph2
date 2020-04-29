using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace ModelGraph.Core
{
    /// <summary>Flat list of LineModel that emulates a UI tree view</summary>
    public class TreeModel : LineModel, IModel
    {
        public Item RootItem => Item;
        public IPageControl PageControl { get; set; } // reference the UI PageControl       
        public ControlType ControlType { get; private set; }

        public string TitleName => "There isn't any Title Name yet";
        public string TitleSummary => "There isn't any Title Summary yet";

        public LineModel SelectModel; // by convention, there will be one selected lineModel (unless of course the lineModel tree hieracy is empty) 
        public int ViewCapacity; // current max number of lines that can be displayed in the UI window 

        public List<LineModel> ViewList = new List<LineModel>(); //flat list view section of model tree hierarchy
        internal int viewListIndex; // index into the flattend model tree hierarchy that corresponds to where the ViewList starts

        internal Dictionary<LineModel, FilterSort> LineModel_FilterSort = new Dictionary<LineModel, FilterSort>();

        #region Constructor  ==================================================
        internal TreeModel(Chef chef) // invoked within RootTreeModel constructor
        {
            Owner = Item = chef;
            ControlType = ControlType.PrimaryTree;
 
            chef.Add(this);
        }
        internal TreeModel(RootTreeModel rootModel, Chef chef) // created by the TreeRootModel
        {
            Owner = rootModel;
            Item = chef;
            ControlType = ControlType.PartialTree;

            chef.Add(this);
        }
        #endregion

        #region IModel  =======================================================
        public void Release()
        {
            if (Owner is null) return;

            DataChef.Remove(this);
            Discard(); //discard myself and recursivly discard all my children

            if (this is RootTreeModel)
                DataChef.Discard(); //kill off the dataChef

            Owner = null;
        }
        #endregion
    }

    #region FilterSort  ===================================================
    internal class FilterSort
    {
        internal string FilterSting;
        internal LineModel Model;
        internal List<LineModel> ViewModels;

        internal FilterSort(LineModel m)
        {
            Model = m;
        }
    }
    #endregion
}
