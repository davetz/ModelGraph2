
namespace ModelGraph.Core
{
    public class X618_CheckPropertyModel : PropertyModel
    {
        internal X618_CheckPropertyModel(LineModel owner, Item item, Property prop) : base(owner, item, prop) { }
        internal override IdKey IdKey => IdKey.CheckPropertyModel;
        public override bool IsCheckModel => true;

        public override (string kind, string name, int count) GetLineParms(Chef chef)
        {
            var name = Property.GetSingleNameId(chef);
            return (null, name, 0);
        }
        public override bool GetBoolValue(Chef chef) => Property.Value.GetBool(Item);
    }
}
