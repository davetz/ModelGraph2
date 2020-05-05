using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{
    public partial class Chef : StoreOf<Store>
    {
        public IRepository Repository { get; set; }
        public static LineModel DragDropSource;
        internal override IdKey IdKey => IdKey.DataChef;

        internal Chef(bool createTestModel = false)
        {
            InitializeDomains(); 

            if (createTestModel) CreateTestModel();
        }

        internal string TitleName => Repository.Name;
        internal string TitleSummary => Repository.FullName;

    }
}
