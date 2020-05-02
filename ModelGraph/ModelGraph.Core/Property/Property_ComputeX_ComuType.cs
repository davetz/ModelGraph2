
namespace ModelGraph.Core
{
    public class Property_ComputeX_CompuType : PropertyOf<ComputeX, string>
    {
        internal override IdKey ViKey => IdKey.ComputeXCompuTypeProperty;

        internal Property_ComputeX_CompuType(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_CompuType>().GetEnumName(chef, (int)Cast(item).CompuType); }
        internal override void SetValue(Item item, string val) { var chef = DataChef; Cast(item).CompuType = (CompuType)chef.Get<Enum_CompuType>().GetKey(chef, val); }
        internal override string GetParentName(Chef chef, Item item) => chef.Get<Relation_Store_ComputeX>().TryGetParent(item, out Store p) ? p.GetSingleNameId(chef) : InvalidItem;
    }
}
