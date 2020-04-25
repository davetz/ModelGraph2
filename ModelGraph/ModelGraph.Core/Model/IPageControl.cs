
namespace ModelGraph.Core
{
    public interface IPageControl
    {
        void Refresh();
        void CreateNewPage(IModel model, ControlType ctlType);
    }
}
