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

        public string TitleName => DataChef.TitleName;
        public string TitleSummary => DataChef.TitleSummary;

        internal Dictionary<LineModel, FilterSort> LineModel_FilterSort = new Dictionary<LineModel, FilterSort>();

        #region Constructor  ==================================================
        internal TreeModel(Chef chef) // invoked within RootTreeModel constructor
        {
            Owner = Item = chef;
            ControlType = ControlType.PrimaryTree;

            chef.Add(this);

            Add(new X612_DataChefModel(this, chef));
            RefreshViewList(null, 5, 0, ChangeType.NoChange);
        }
        internal TreeModel(RootTreeModel rootModel, Chef chef, IdKey childId) // created by the TreeRootModel
        {
            Owner = rootModel;
            Item = chef;
            ControlType = ControlType.PartialTree;

            chef.Add(this);
            switch (childId)
            {
                case IdKey.MetadataRootModel:
                    new X623_MetadataRootModel(this, chef);
                    break;
                case IdKey.ModelingRootModel:
                    new X623_MetadataRootModel(this, chef);
                    break;
                case IdKey.ChangeRootModel:
                    new X622_ChangeRootModel(this, chef);
                    break;
                case IdKey.ErrorRootModel:
                    new X621_ErrorRootModel(this, chef);
                    break;
                default:
                    throw new ArgumentException($"TreeModel constructor, Invalid IdKey child: {childId}");
            }
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

        #region RefreshViewList  ==============================================
        // Runs on a background thread invoked by the ModelTreeControl 
        private int _fullyFlattenedSize; // flattened tree hierachy length (from the last full traversal)
        private int _bufferStartIndex; // start index into the fully flattend model tree hierarchy
        private List<LineModel> _bufferList = new List<LineModel>(100); //flat list view buffer, 3 x number of visible lines (function of UI-Window height)
        private int _viewStartIndex; // start index into the bufferList
        private int _viewSize; // number of viewable lines in UI window
        private LineModel _selectModel; //the UI is now or will be forced to be focused on this model
        public (List<LineModel>, LineModel) GetCurrentView() => (_bufferList.GetRange(_viewStartIndex, _viewSize), _selectModel);

        public void RefreshViewList(LineModel select, int viewSize, int scroll = 0, ChangeType change = ChangeType.NoChange)
        {
            _selectModel = select;
            _viewSize = viewSize;

            var cap = viewSize * 3; // my buffer size = 3 x number of visible lines (from UI window height)
            if (cap > _bufferList.Capacity) _bufferList.Capacity = cap;

            if (ChildDelta != DataChef.ChildDelta) //anything added/moved/removed/linked/unlinked in dataChef
            {
                ChildDelta = DataChef.ChildDelta;
            }

            //    if (capacity > 0)
            //    {
            //        var first = IModel.FirstValidModel(viewList);
            //        var start = (first == null);
            //        var previous = new List<IModel>();
            //        var modelStack = new TreeModelStack();

            //        UpdateSelectModel(select, change);

            //        if (root.IsForcedRefresh)
            //        {
            //            modelStack.PushRoot(root);
            //        }
            //        else
            //        {
            //            if (root.ChildModelCount == 0)
            //            {
            //                root.Validate(previous);
            //                root.ViewModels = root.ChildModels;
            //            }
            //            modelStack.PushChildren(root);
            //        }

            //        var S = (scroll < 0) ? -scroll : scroll;
            //        var N = capacity;
            //        var buffer = new CircularBuffer(N, S);

            //        #region GoTo<End,Home>  =======================================
            //        if ((change == ChangeType.GoToEnd || change == ChangeType.GoToHome) && offset >= 0 && first != null)
            //        {
            //            var pm = select.ParentModel;
            //            var ix = pm.GetChildlIndex(select);
            //            var last = pm.ChildModelCount - 1;

            //            if (change == ChangeType.GoToEnd)
            //            {
            //                if (ix < last)
            //                {
            //                    select = pm.ViewModels[last];
            //                    if (!viewList.Contains(select)) FindFirst();
            //                }
            //            }
            //            else
            //            {
            //                if (ix > 0)
            //                {
            //                    select = pm.ViewModels[0];
            //                    if (!viewList.Contains(select)) FindFirst();
            //                }
            //            }
            //            root.SelectModel = select;

            //            void FindFirst()
            //            {
            //                first = select;
            //                var absoluteFirst = root.ViewModels[0];

            //                for (; offset > 0; offset--)
            //                {
            //                    if (first == absoluteFirst) break;

            //                    var p = first.ParentModel;
            //                    var i = p.GetChildlIndex(first);

            //                    first = (i > 0) ? p.ViewModels[i - 1] : p;
            //                }
            //            }

        }
        #endregion

        #region TraverseModelHierarchy  =======================================
        private void TraverseModelHierarchy()
        {
            var bufferCap = _bufferList.Capacity;
            var startIndex = _bufferStartIndex;


        }
        #endregion


        #region RequieredMethods  =============================================
        internal override (bool anyChange, int flatCount) Validate() => (false, 0);
        public override (string kind, string name, int count) GetLineParms(Chef chef) => (null, BlankName, 0);
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
