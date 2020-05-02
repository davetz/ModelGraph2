using ModelGraph.Controls;
using ModelGraph.Core;
using ModelGraph.Repository;
using ModelGraph.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace ModelGraph.Services
{
    public class ModelPageService
    {
        public static ModelPageService Current => _current ?? (_current = new ModelPageService());
        private static ModelPageService _current;

        #region Constructor  ==================================================
        private ModelPageService()
        {
            ApplicationView.GetForCurrentView().Consolidated += ModelPageService_Consolidated;
        }

        private void ModelPageService_Consolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            var app = (App)Application.Current;
            app.Exit(); // all ModelPage will close
        }
        #endregion

        #region Dispatch  =====================================================
        //public async Task<bool> Dispatch(UIRequestOld rq, IModelPageControl ctrl)
        //{
        //    if (rq is null || ctrl is null) return false;

        //    switch (rq.RequestType)
        //    {
        //        case RequestType.Apply:
        //            await ctrl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { ctrl.Apply(); });
        //            return true;

        //        case RequestType.Revert:
        //            await ctrl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { ctrl.Revert(); });
        //            return true;

        //        case RequestType.Save:
        //            await ctrl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { ctrl.IModel.Chef.Repository.SaveAsync(ctrl.IModel.Chef); });
        //            return true;

        //        case RequestType.SaveAs:
        //            await ctrl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { ctrl.IModel.Chef.Repository.SaveAS(ctrl.IModel.Chef); });
        //            return true;

        //        case RequestType.Refresh:
        //            await ctrl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { ctrl.Refresh(); });
        //            return true;

        //        case RequestType.Reload:
        //            await ctrl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { _ = ReloadModelAsync(ctrl); });
        //            return true;


        //        case RequestType.Close:
        //            await ctrl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { RemoveModelPage(ctrl.IModel); WindowManagerService.Current.CloseRelatedModels(ctrl.IModel); ctrl.Release(); });
        //            return true;

        //        case RequestType.CreateView:
        //            await ctrl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => InsertModelPage(new IModel(rq)));
        //            return true;

        //        case RequestType.CreatePage:
        //            var rootModel = new IModel(rq);
        //            var viewLifetimeControl = await WindowManagerService.Current.TryShowAsStandaloneAsync(rootModel.TitleName, typeof(ModelPage), rootModel).ConfigureAwait(true);
        //            viewLifetimeControl.Released += ViewLifetimeControl_Released;
        //            return true;
        //    }
        //    GC.Collect();
        //    return false;
        //}
        internal async Task CreateNewPageAsync(IModel model, ControlType ctlType)
        {
            var viewLifetimeControl = await WindowManagerService.Current.TryShowAsStandaloneAsync(model.TitleName, typeof(ModelPage), model).ConfigureAwait(true);
            viewLifetimeControl.Released += ViewLifetimeControl_Released;
        }

        private void ViewLifetimeControl_Released(object sender, EventArgs e)
        {
            if (sender is ViewLifetimeControl ctrl)
            {
                ctrl.Released -= ViewLifetimeControl_Released;
                var modelControl = ctrl.IModel?.PageControl as IModelPageControl;
                modelControl?.Release();
            }
        }
        #endregion

        #region ReloadModel  ==================================================
        public async Task<bool> ReloadModelAsync(IModelPageControl ctrl)
        {
            if (ctrl is null) return false;

            var oldRootModel = ctrl.IModel;
            var oldChef = oldRootModel.DataChef;
            var repo = oldChef.Repository;

            RemoveModelPage(oldRootModel);
            oldRootModel.Release();

            WindowManagerService.Current.CloseRelatedModels(oldRootModel);

            var rootModel = ModelGraphCore.CreateRootTreeModel();
            var newChef = rootModel.DataChef;

            _ = await repo.ReloadAsync(newChef).ConfigureAwait(true);

            await ctrl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                InsertModelPage(rootModel);
            });

            return true;
        }
        #endregion

        #region CreateNewModel  ===============================================
        public async Task<bool> CreateNewModelAsync(CoreDispatcher dispatcher)
        {
            if (dispatcher is null) return false;

            var rootModel = ModelGraphCore.CreateRootTreeModel();
            var repo = new StorageFileRepo();
            repo.New(rootModel.DataChef);

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                InsertModelPage(rootModel);
            });

            return true;
        }
        #endregion

        #region OpenModelDataFile  ============================================\
        public async Task<bool> OpenModelDataFileAsync(CoreDispatcher dispatcher)
        {
            if (dispatcher is null) return false;

            var rootModel = ModelGraphCore.CreateRootTreeModel();
            var repo = new StorageFileRepo();
            bool success = await repo.OpenAsync(rootModel.DataChef).ConfigureAwait(true);


            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                InsertModelPage(rootModel);
            });

            return success;
        }
        #endregion

        public Action<IModel> InsertModelPage { get; set; } //coordination with ShellPage NavigationView
        public Action<IModel> RemoveModelPage { get; set; } //coordination with ShellPage NavigationView
    }
}
