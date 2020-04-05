
namespace ModelGraph.Core
{
    // Programmatic command the ModelGraph UI
    public enum RequestType
    {
        Save,       // save to repository
        Close,      // close the data model
        Apply,      // apply editor changes to model data
        Revert,     // revert to model data in the editor
        Reload,     // reload model from the repository
        SaveAs,     // save model to a new repository
        Refresh,    // refress the display from model data
        CreateView, // create another model view in the main window
        CreatePage, // create a new window model page
    }
}
