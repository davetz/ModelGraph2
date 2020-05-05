using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace ModelGraph.Core
{
    public class RootTreeModel : TreeModel
    {

        #region Constructor  ==================================================
        internal RootTreeModel() : base (new Chef(), IdKey.DataChefModel) { }
        internal override void Add(Item item)
        {
            if (item is X612_DataChefModel) //I can only have this one special child model
                base.Add(item);
        }
        #endregion


        #region CreateMetadataSubRootModel  ===================================
        internal void CreateMetadataSubRootModel()
        {
            var chef = Item as Chef;
            var tm = new TreeModel(this, chef);
            //new X62E_CreateMetadataSubRootModel(tm, chef); 
        }
        #endregion
    }
}
