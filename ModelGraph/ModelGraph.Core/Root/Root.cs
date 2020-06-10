
namespace ModelGraph.Core
{
    public partial class Root : StoreOf<Store>
    {
        public IRepository Repository { get; set; }
        public static LineModel DragDropSource;
        internal override IdKey IdKey => IdKey.DataChef;

        internal Root(bool createTestModel = false)
        {
            ModelDelta = ChildDelta = 1;

            InitializeDataRoots(); 

            if (createTestModel) CreateTestModel();
        }

        internal string TitleName => Repository.Name;
        internal string TitleSummary => Repository.FullName;
    }
}
