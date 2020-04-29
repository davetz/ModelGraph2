using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Core
{
    public class X600_ParmDebugListModel : LineModel
    {
        internal X600_ParmDebugListModel(LineModel owner, Item item) : base(owner, item) { }

        #region Override Methodes  ============================================

        internal override IdKey ViKey => IdKey.ParmDebugListModel;
        public override (string Kind, string Name, int Count, ModelType Type) GetLineParms(Chef chef)
        {
            var (kind, name) = GetKindNameId(chef);
            CanExpandLeft = true;
            return (kind, name, 0, ModelType.Default);
        }

        internal override (bool, bool) Validate(Chef chef)
        {
            if (Count == 1) return (true, false);

            new X617_TextPropertyModel(this, chef, chef.ShowItemIndexProperty);

            return (true, true);
        }
        #endregion
    }
}
