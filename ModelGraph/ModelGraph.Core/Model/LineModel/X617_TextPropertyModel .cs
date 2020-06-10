
namespace ModelGraph.Core
{
    public class X617_TextPropertyModel : PropertyModel
    {
        internal X617_TextPropertyModel(LineModel owner, Item item, Property prop) : base(owner, item, prop) { }
        internal override IdKey IdKey => IdKey.TextPropertyModel;
        public override bool IsTextModel => true;

        public override (string kind, string name, int count) GetLineParms(Root root)
        {
            var name = Property.GetSingleNameId(root);
            return (null, name, 0);
        }
        public override string GetTextValue(Root root) => Property.Value.GetString(Item);
    }
}
