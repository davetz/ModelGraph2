
namespace ModelGraph.Core
{
    public class Property_Relation_Pairing : PropertyOf<Relation, string>
    {
        internal override IdKey IdKey => IdKey.RelationPairingProperty;

        internal Property_Relation_Pairing(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var root = DataRoot; return root.Get<Enum_Pairing>().GetEnumName(root, (int)Cast(item).Pairing); }
        internal override void SetValue(Item item, string val) { var root = DataRoot; Cast(item).TrySetPairing((Pairing)root.Get<Enum_Pairing>().GetKey(root, val)); }
    }
}
