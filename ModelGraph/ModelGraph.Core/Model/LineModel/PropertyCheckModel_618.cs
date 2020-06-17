
namespace ModelGraph.Core
{
    public class PropertyCheckModel_618 : PropertyModel
    {
        internal PropertyCheckModel_618(LineModel owner, Item item, Property prop) : base(owner, item, prop) { }
        internal override IdKey IdKey => IdKey.PropertyCheckModel_618;
        public override bool IsCheckModel => true;

        public override (string kind, string name, int count) GetLineParms(Root root)
        {
            var name = Property.GetSingleNameId(root);
            return (null, name, 0);
        }
        public override bool GetBoolValue(Root root) => Property.Value.GetBool(Item);
    }
}
