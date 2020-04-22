﻿
namespace ModelGraph.Core
{/*
    This is how the ModelGraph.Core requests UI control action.
 */
    public class UIRequest
    {
        // parameters for new view
        public RootModel Root { get; private set; }
        internal Item Item { get; private set; }
        internal Item Aux1 { get; private set; }
        internal Item Aux2 { get; private set; }
        public ControlType Type { get; private set; }
        internal ModelAction Get { get; private set; }
        internal IdKey IdKey { get; private set; }
        public RequestType RequestType { get; private set; }

        private UIRequest() { }

        internal static UIRequest SaveModel(RootModel root)
        {
            return new UIRequest()
            {
                Root = root,
                RequestType = RequestType.Save
            };
        }
        public static UIRequest ApplyModel(RootModel root)
        {
            return new UIRequest()
            {
                Root = root,
                RequestType = RequestType.Apply
            };
        }
        public static UIRequest RevertModel(RootModel root)
        {
            return new UIRequest()
            {
                Root = root,
                RequestType = RequestType.Revert
            };
        }
        public static UIRequest CloseModel(RootModel root)
        {
            return new UIRequest()
            {
                Root = root,
                RequestType = RequestType.Close
            };
        }
        internal static UIRequest ReloadModel(RootModel root)
        {
            return new UIRequest()
            {
                Root = root,
                RequestType = RequestType.Reload
            };
        }
        internal static UIRequest SaveAsModel(RootModel root)
        {
            return new UIRequest()
            {
                Root = root,
                RequestType = RequestType.SaveAs
            };
        }
        internal static UIRequest SaveSymbol(RootModel root)
        {
            return new UIRequest()
            {
                Root = root,
                RequestType = RequestType.Save
            };
        }
        internal static UIRequest ReloadSymbol(RootModel root)
        {
            return new UIRequest()
            {
                Root = root,
                RequestType = RequestType.Reload
            };
        }
        internal static UIRequest RefreshModel(RootModel root)
        {
            return new UIRequest()
            {
                Root = root,
                RequestType = RequestType.Refresh
            };
        }
        internal static UIRequest CreateView(RootModel root, ControlType type, IdKey idKe, Item item, ModelAction modelAction)
        {
            return new UIRequest()
            {
                Root = root,
                Type = type,
                IdKey = idKe,
                Item = item,
                Get = modelAction,
                RequestType = RequestType.CreateView,
            };
        }
        internal static UIRequest CreatePage(RootModel root, ControlType type, IdKey idKe, Item item, Item aux1, Item aux2, ModelAction modelAction)
        {
            return new UIRequest()
            {
                Root = root,
                Type = type,
                IdKey = idKe,
                Item = item,
                Aux1 = aux1,
                Aux2 = aux2,
                Get = modelAction,
                RequestType = RequestType.CreatePage,
            };
        }
    }
}
