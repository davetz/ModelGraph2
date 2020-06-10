
namespace ModelGraph.Core
{
    public class Property_ComputeX_CompuType : PropertyOf<ComputeX, string>
    {
        internal override IdKey IdKey => IdKey.ComputeXCompuTypeProperty;

        internal Property_ComputeX_CompuType(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var root = DataChef; return root.Get<Enum_CompuType>().GetEnumName(root, (int)Cast(item).CompuType); }
        internal override void SetValue(Item item, string val) { var root = DataChef; Cast(item).CompuType = (CompuType)root.Get<Enum_CompuType>().GetKey(root, val); }
        internal override string GetParentName(Root root, Item item) => root.Get<Relation_Store_ComputeX>().TryGetParent(item, out Store p) ? p.GetSingleNameId(root) : InvalidItem;
    }
}
