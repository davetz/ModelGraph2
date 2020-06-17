
namespace ModelGraph.Core
{
    public class PropertyComboModel_619 : PropertyModel
    {
        internal PropertyComboModel_619(LineModel owner, Item item, Property prop) : base(owner, item, prop) { }
        internal override IdKey IdKey => IdKey.PropertyComboModel_619;
        public override bool IsComboModel => true;

        public override (string kind, string name, int count) GetLineParms(Root root)
        {
            var name = Property.GetSingleNameId(root);
            return (null, name, 0);
        }
        public override int GetIndexValue(Root root) => Property.GetIndexValue(Item);
        public override string[] GetlListValue(Root root) => Property.GetlListValue(root);
    }
}
