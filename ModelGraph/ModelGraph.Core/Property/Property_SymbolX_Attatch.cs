
namespace ModelGraph.Core
{
    public class Property_SymbolX_Attatch : PropertyOf<SymbolX, string>
    {
        internal override IdKey IdKey => IdKey.SymbolXAttatchProperty;

        internal Property_SymbolX_Attatch(PropertyRoot owner)
        {
            Owner = owner;
            Value = new StringValue(this);

            owner.Add(this);
        }

        internal override string GetValue(Item item) { var root = DataRoot; return root.Get<Enum_Attach>().GetEnumName(root, (int)Cast(item).Attach); }
        internal override void SetValue(Item item, string val) { var root = DataRoot; Cast(item).Attach = (Attach)root.Get<Enum_Attach>().GetKey(root, val); }
    }
}
