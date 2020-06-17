﻿
namespace ModelGraph.Core
{
    public interface IModel
    {
        Root DataRoot { get; }          // dataChef
        Item RootItem { get; }
        RootModel RootTreeModel { get; }   // there is exactly one rootTreeModel per dataChef
        string TitleName { get; }       // title that appears in the windows title bar
        string TitleSummary { get; }       // title that appears in the windows title bar
        ControlType ControlType { get; } // tells the UI what kind control to create for this model
        IPageControl PageControl { get; set; } //set by the UI
        void Release();                 // release all references and remove this model from memory
    }
}
