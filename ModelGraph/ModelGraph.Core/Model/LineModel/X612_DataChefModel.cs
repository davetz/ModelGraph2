
namespace ModelGraph.Core
{
    public class X612_DataChefModel : LineModel
    {
        public override bool CanExpandLeft => true;

        internal X612_DataChefModel(LineModel owner, Item item) : base(owner, item) 
        { 
        }


        #region Override Methodes  ============================================
        internal override IdKey ViKey => IdKey.DataChefModel;
        public override (string Kind, string Name, int Count, ModelType Type) GetParms(Chef chef)
        {
            var (kind, name) = GetKindNameId(chef);
            return (kind, name, 0, ModelType.Default);
        }

        internal override (bool, bool) Validate(Chef chef)
        {
            return (true, false);
        }
        #endregion
    }
}
