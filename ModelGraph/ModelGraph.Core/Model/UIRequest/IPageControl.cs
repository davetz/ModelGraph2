
namespace ModelGraph.Core
{/*
    interface between ModelGraph.Core --> ModelGraph

    Isolate the ModelGraph.Core from the UI plumbing.
 */
    public interface IPageControl
    {
        void Dispatch(UIRequest request);
    }
}
