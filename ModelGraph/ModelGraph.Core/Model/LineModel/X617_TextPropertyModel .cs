using System;
using System.Collections.Generic;
using System.Text;

namespace ModelGraph.Core
{
    public class X617_TextPropertyModel : LineModel
    {
        internal Item Item;
        internal Property Aux1;

        internal X617_TextPropertyModel(LineModel owner, Item item, Property aux) : base(owner, item) 
        {
            Item = item;
            Aux1 = aux;
        }

        #region Override Methodes  ============================================
        internal override IdKey ViKey => IdKey.TextPropertyModel;
        public override (string Kind, string Name, int Count, ModelType Type) GetLineParms(Chef chef)
        {
            var (kind, name) = GetKindNameId(chef);
            CanExpandLeft = true;
            return (kind, name, 0, ModelType.Default);
        }

        public override string GetPropertyTextValue(Chef chef) => Aux1.Value.GetString(Item);

        public override (string, string) GetKindNameId(Chef chef)
        {
            var name = Aux1.GetSingleNameId(chef);

            return (null, Aux1.HasItemName ? Aux1.GetItemName(Item) : name);
        }

        public override string GetModelIdentity() => $"{ViKey}  ({ItemKey:X3}){Environment.NewLine}{Aux1.ViKey}  ({Aux1.ItemKey:X3})";
        #endregion
    }
}
