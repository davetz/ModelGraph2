using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public partial class Chef : StoreOf<Store>
    {
        public IRepository Repository { get; set; }
        public static ItemModel DragDropSource; 

        private bool ShowItemIndex;

        #region Constructor  ==================================================
        internal Chef(bool createTestModel = false) : base(null, Trait.DataChef, 0)
        {
            Initialize();

            if (createTestModel) CreateTestModel();
        }
        #endregion

        #region RootModels  ===================================================
        private RootModel PrimaryRootModel;
        private List<RootModel> _rootModels = new List<RootModel>(10);
        internal void AddRootModel(RootModel root)
        {
            if (PrimaryRootModel == null) PrimaryRootModel = root;
            _rootModels.Add(root);
        }
        internal void RemoveRootModel(RootModel root)
        {
            if (_rootModels is null) return;

            _rootModels.Remove(root);
            if (root.Item is Graph g)
            {
                if (_graphRefereceCount.TryGetValue(g, out int count))
                {
                    if (count - 1 > 0)
                        _graphRefereceCount[g] = count - 1;
                    else
                    {
                        _graphRefereceCount.Remove(g);
                        var gx = g.GraphX;
                        gx.Remove(g);
                        PostRefresh(PrimaryRootModel);
                    }
                }
            }
        }
        private void RegisterGraphInstance(Graph g)
        {
            if (_graphRefereceCount.TryGetValue(g, out int count))
                _graphRefereceCount[g] = count + 1;
            else
                _graphRefereceCount.Add(g, 1);
        }
        private Dictionary<Graph, int> _graphRefereceCount = new Dictionary<Graph, int>(4);
        #endregion

        #region Initialize  ===================================================
        private void Initialize()
        {
            ClearItemErrors();
            InitializeItemIdentity();

            InitializeStores();
            InitializeRelations();

            InitializeEnums();
            InitializeProperties();

            InitializeReferences();

            InitializeModelActions();
        }
        #endregion
    }
}
