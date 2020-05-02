
namespace ModelGraph.Core
{
    public class Property_Relation_Pairing : PropertyOf<Relation, string>
    {
        internal override IdKey ViKey => IdKey.ColumnValueTypeProperty;

        internal Property_Relation_Pairing(StoreOf_Property owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var chef = DataChef; return chef.Get<Enum_Pairing>().GetEnumName(chef, (int)Cast(item).Pairing); }
        internal override void SetValue(Item item, string val) { var chef = DataChef; Cast(item).TrySetPairing((Pairing)chef.Get<Enum_Pairing>().GetKey(chef, val)); }
    }
}
