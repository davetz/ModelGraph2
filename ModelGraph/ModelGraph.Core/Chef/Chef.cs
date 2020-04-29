﻿using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public partial class Chef : StoreOf<Store>
    {
        public IRepository Repository { get; set; }
        public static LineModel DragDropSource; 

        private bool ShowItemIndex;

        #region Constructor  ==================================================
        internal Chef(bool createTestModel = false) : base(null, IdKey.DataChef, 0)
        {
            Initialize();

            if (createTestModel) CreateTestModel();
        }
        #endregion

        #region RootModels  ===================================================
        private RootModelOld PrimaryRootModel;
        private List<RootModelOld> _rootModels = new List<RootModelOld>(10);
        internal void AddRootModel(RootModelOld root)
        {
            if (PrimaryRootModel == null) PrimaryRootModel = root;
            _rootModels.Add(root);
        }
        internal void RemoveRootModel(RootModelOld root)
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

            InitializeDomains();
            InitializeRelations();

            CreateProperties();

            InitializeReferences();

            InitializeModelActions();
        }
        #endregion

        #region Release  ======================================================
        internal void Release()
        {
            Items.Clear();
        }
        #endregion
    }
}
