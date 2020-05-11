
namespace ModelGraph.Core
{
    public class X619_ComboPropertyModel : PropertyModel
    {
        internal X619_ComboPropertyModel(LineModel owner, Item item, Property prop) : base(owner, item, prop) { }
        internal override IdKey IdKey => IdKey.ComboPropertyModel;
        public override bool IsComboModel => true;

        public override (string kind, string name, int count) GetLineParms(Chef chef)
        {
            var name = Property.GetSingleNameId(chef);
            return (null, name, 0);
        }
        public override int GetIndexValue(Chef chef) => Property.GetIndexValue(chef, Item);
        public override string[] GetlListValue(Chef chef) => Property.GetlListValue(chef);
    }
}
