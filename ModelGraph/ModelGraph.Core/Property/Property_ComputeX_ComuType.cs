
namespace ModelGraph.Core
{
    public class Property_ComputeX_CompuType : PropertyOf<ComputeX, string>
    {
        internal override IdKey ViKey => IdKey.ComputeXCompuTypeProperty;

        internal Property_ComputeX_CompuType(PropertyDomain owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Chef chef, Item item) => chef.GetItem<Enum_CompuType>().GetEnumName(chef, (int)Cast(item).CompuType);
        internal override void SetValue(Chef chef, Item item, string val) => Cast(item).CompuType = (CompuType)chef.GetItem<Enum_CompuType>().GetKey(chef, val);
        internal override string GetParentName(Chef chef, Item item) => chef.GetItem<Relation_Store_ComputeX>().TryGetParent(item, out Store p) ? p.GetSingleNameId(chef) : InvalidItem;
    }
}
